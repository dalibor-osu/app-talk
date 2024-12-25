use std::collections::HashMap;
use std::error::Error;
use std::ptr::null;

use crossterm::event::{self, Event};
use ratatui::{style::Style, text::Text, Frame};
use crossterm::event::{DisableMouseCapture, EnableMouseCapture};
use crossterm::terminal::{
    disable_raw_mode, enable_raw_mode, EnterAlternateScreen, LeaveAlternateScreen,
};
use ratatui::backend::CrosstermBackend;
use ratatui::layout::{Constraint, Layout};
use ratatui::widgets::{Block, Borders};
use ratatui::Terminal;
use reqwest::Client;
use std::{default, io};
use tui_textarea::{Input, Key, TextArea};
use serde_json::json;
use futures::executor;
use tokio;

macro_rules! hashmap {
    ($( $key: expr => $val: expr ),*) => {{
         let mut map = ::std::collections::HashMap::new();
         $( map.insert($key, $val); )*
         map
    }}
}

pub trait Scene {
    fn setup(&mut self);
    fn update(&mut self);
    fn render(&self, frame: &mut Frame);
    fn handle_input(&mut self) -> io::Result<bool>;
}

use ratatui::prelude::*;

struct App<'a> {
    scenes: HashMap<&'a str, Box<dyn Scene + 'a>>,
}

struct RegisterScene<'a> {
    children: HashMap<&'a str, TextArea<'a>>,
    focused_area: &'a str
}

impl<'a> RegisterScene<'a> {
    fn new<'b>() -> RegisterScene<'b> {
        return RegisterScene {
            children: hashmap![
                "username" => TextArea::default(),
                "password" => TextArea::default(),
                "email" => TextArea::default(),
                "result" => TextArea::default()
            ],
            focused_area: "username"
        }
    }

    fn child_get_mut(&mut self, key: &str) -> &mut TextArea<'a> {
        return self.children.get_mut(key).unwrap();
    }

    fn get_focused_child(&mut self) -> &mut TextArea<'a> {
        return self.child_get_mut(self.focused_area);
    }

    fn swap_focused_child(&mut self, next: &'a str) {
        let current_child = self.get_focused_child();
        let visible_style = current_child.cursor_style();
        current_child.set_cursor_style(Style::new().add_modifier(Modifier::HIDDEN));

        let next_child = self.child_get_mut(next);
        next_child.set_cursor_style(visible_style);

        self.focused_area = next;
    }

    fn next_child(&mut self) {
        match self.focused_area {
            "username" => self.swap_focused_child("password"),
            "password" => self.swap_focused_child("email"),
            "email" => self.swap_focused_child("username"),
            _ => self.swap_focused_child("username"),
        }
    }

    async fn submit(&mut self) -> Result<bool, Box<dyn std::error::Error>> {
        let url = "http://localhost:5209/api/user/register";
        let username = self.children["username"].lines().first();
        let email = self.children["email"].lines().first();
        let password = self.children["password"].lines().first();
        let client = Client::new();
        let payload = json!({
            "username": username,
            "email": email,
            "password": password
        });

        let response = client.post(url)
            .json(&payload)
            .send()
            .await?
            .json::<serde_json::Value>()
            .await?;

        let message = serde_json::to_string_pretty(&response).unwrap();
        self.child_get_mut("result").insert_str(message);
        Ok(true)
    }
}

impl<'a> Scene for RegisterScene<'a> {
    fn setup(&mut self) {
        let password = self.child_get_mut("password");
        password.set_cursor_style(Style::default().add_modifier(Modifier::HIDDEN));
        password.set_mask_char('\u{2022}');
        password.set_block(Block::bordered().title("Password"));

        let email = self.child_get_mut("email");
        email.set_cursor_style(Style::default().add_modifier(Modifier::HIDDEN));
        email.set_block(Block::bordered().title("Email"));

        let username = self.child_get_mut("username");
        username.set_block(Block::bordered().title("Username"));

        let result = self.child_get_mut("result");
        result.set_cursor_style(Style::default().add_modifier(Modifier::HIDDEN));
        result.set_block(Block::bordered().title("Result"));
    }

    fn update(&mut self) {
        return;
    }

    fn render(&self, frame: &mut Frame) {
        let layout = Layout::default()
            .direction(Direction::Vertical)
            .constraints(vec![
                Constraint::Percentage(33),
                Constraint::Percentage(34),
                Constraint::Percentage(33),
            ])
            .split(frame.area());

        let inner_layout = Layout::default()
            .direction(Direction::Horizontal)
            .constraints(vec![
                Constraint::Percentage(33),
                Constraint::Percentage(34),
                Constraint::Percentage(33),
            ])
            .split(layout[1]);

        let middle_layout = Layout::default()
            .direction(Direction::Vertical)
            .flex(layout::Flex::Center)
            .constraints(vec![
                Constraint::Length(3),
                Constraint::Length(3),
                Constraint::Length(3),
            ])
            .split(inner_layout[1]);

        frame.render_widget(&self.children["email"], middle_layout[0]);
        frame.render_widget(&self.children["username"], middle_layout[1]);
        frame.render_widget(&self.children["password"], middle_layout[2]);

        frame.render_widget(&self.children["result"], layout[2]);
    }

    fn handle_input(&mut self) -> io::Result<bool> {
        match crossterm::event::read()?.into() {
            Input {
                key: Key::Esc,
                ..
            } => Ok(false),
            Input {
                key: Key::Tab,
                ..
            } => {
                self.next_child();
                Ok(true)
            },
            Input {
                key: Key::Enter,
                ..
            } => {
                executor::block_on(self.submit());
                Ok(true)
            },
            input => {
                let child = self.children.get_mut(self.focused_area).unwrap();
                if child.input(input) {
                }
                Ok(true)
            }
        }
    }
}

#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    let stdout = io::stdout();
    let mut stdout = stdout.lock();

    enable_raw_mode()?;
    crossterm::execute!(stdout, EnterAlternateScreen, EnableMouseCapture)?;

    let backend = CrosstermBackend::new(stdout);
    let mut term = Terminal::new(backend)?;
    let mut register: Box<dyn Scene> = Box::new(RegisterScene::new());
    register.setup();

    loop {
        term.draw(|f: &mut Frame| register.render(f)).expect("failed");
        let result = register.handle_input();
        if !result.unwrap() {
            break;
        }
    }
    disable_raw_mode()?;
    crossterm::execute!(
        term.backend_mut(),
        LeaveAlternateScreen,
        DisableMouseCapture,
    )?;
    term.show_cursor()?;
    Ok(())
}
