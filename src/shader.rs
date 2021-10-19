#![allow(non_snake_case)]
use std::ffi::{CStr, CString};
use std::fs::File;
use std::io::Read;
use std::ptr;
use std::str;

use gl::types::{*, self};

pub struct Shader {
    pub id: u32,
}

fn str_to_cstring(str: &str) -> CString {
    CString::new(str).unwrap()
}

#[allow(dead_code)]
impl Shader {
    pub fn new(vertex_path: &str, fragment_path: &str) -> Shader {
        let mut shader = Shader { id: 0 };
        // 1. retrieve the vertex/fragment source code from filesystem
        let mut vertex_shader_file =
            File::open(vertex_path).unwrap_or_else(|_| panic!("Failed to open {}", vertex_path));
        let mut fragment_shader_file = File::open(fragment_path)
            .unwrap_or_else(|_| panic!("Failed to open {}", fragment_path));
        let mut vertexCode = String::new();
        let mut fragmentCode = String::new();
        vertex_shader_file
            .read_to_string(&mut vertexCode)
            .expect("Failed to read vertex shader");
        fragment_shader_file
            .read_to_string(&mut fragmentCode)
            .expect("Failed to read fragment shader");

        let vertex_shader_source = CString::new(vertexCode.as_bytes()).unwrap();
        let fragment_shader_source = CString::new(fragmentCode.as_bytes()).unwrap();

        unsafe {
            // vertex shader
            let vertex = gl::CreateShader(gl::VERTEX_SHADER);
            gl::ShaderSource(vertex, 1, &vertex_shader_source.as_ptr(), ptr::null());
            gl::CompileShader(vertex);
            shader.check_compile_errors(vertex, "VERTEX");
            // fragment Shader
            let fragment = gl::CreateShader(gl::FRAGMENT_SHADER);
            gl::ShaderSource(fragment, 1, &fragment_shader_source.as_ptr(), ptr::null());
            gl::CompileShader(fragment);
            shader.check_compile_errors(fragment, "FRAGMENT");
            // shader Program
            let id = gl::CreateProgram();
            gl::AttachShader(id, vertex);
            gl::AttachShader(id, fragment);
            gl::LinkProgram(id);
            shader.check_compile_errors(id, "PROGRAM");
            // delete the shaders as they're linked into our program now and no longer necessary
            gl::DeleteShader(vertex);
            gl::DeleteShader(fragment);
            shader.id = id;
        }

        shader
    }

    pub unsafe fn use_program(&self) {
        gl::UseProgram(self.id)
    }

    unsafe fn get_uniform_location(&self, name: &str) -> types::GLint {
        gl::GetUniformLocation(self.id, str_to_cstring(name).as_ptr())
    }

    pub unsafe fn set_bool(&self, name: &str, value: bool) {
        gl::Uniform1i(self.get_uniform_location(name), value as i32);
    }

    pub unsafe fn set_int(&self, name: &str, value: i32) {
        gl::Uniform1i(self.get_uniform_location(name), value);
    }

    pub unsafe fn set_float(&self, name: &str, value: f32) {
        gl::Uniform1f(self.get_uniform_location(name), value);
    }

    pub unsafe fn set_vec2(&self, name: &str, value: &(f32, f32)) {
        gl::Uniform2f(
            self.get_uniform_location(name),
            value.0,
            value.1,
        );
    }

    unsafe fn check_compile_errors(&self, shader: u32, type_: &str) {
        let mut success = gl::FALSE as GLint;
        let mut infoLog = Vec::with_capacity(1024);
        infoLog.set_len(1024 - 1); // subtract 1 to skip the trailing null character
        if type_ != "PROGRAM" {
            gl::GetShaderiv(shader, gl::COMPILE_STATUS, &mut success);
            if success != gl::TRUE as GLint {
                gl::GetShaderInfoLog(
                    shader,
                    1024,
                    ptr::null_mut(),
                    infoLog.as_mut_ptr() as *mut GLchar,
                );
                println!(
                    "ERROR::SHADER_COMPILATION_ERROR of type: {}\n{}\n \
                          -- --------------------------------------------------- -- ",
                    type_,
                    str::from_utf8(&infoLog).unwrap()
                );
            }
        } else {
            gl::GetProgramiv(shader, gl::LINK_STATUS, &mut success);
            if success != gl::TRUE as GLint {
                gl::GetProgramInfoLog(
                    shader,
                    1024,
                    ptr::null_mut(),
                    infoLog.as_mut_ptr() as *mut GLchar,
                );
                println!(
                    "ERROR::PROGRAM_LINKING_ERROR of type: {}\n{}\n \
                          -- --------------------------------------------------- -- ",
                    type_,
                    str::from_utf8(&infoLog).unwrap()
                );
            }
        }
    }
}
