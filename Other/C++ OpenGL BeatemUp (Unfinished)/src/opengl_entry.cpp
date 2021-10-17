#include "opengl_entry.hpp"
#include <stdio.h>

unsigned int defaultShader;
GLFWwindow* window;
float aspect_ratio;

void GLAPIENTRY
MessageCallback( GLenum source,
                 GLenum type,
                 GLuint id,
                 GLenum severity,
                 GLsizei length,
                 const GLchar* message,
                 const void* userParam )
{
  fprintf( stderr, "GL CALLBACK: %s type = 0x%x, severity = 0x%x, message = %s\n",
           ( type == GL_DEBUG_TYPE_ERROR ? "** GL ERROR **" : "" ),
            type, severity, message );
}

void opengl_init(int width, int height, char* name)
{
	aspect_ratio = width/(float)height;
    /* Initialize the library */
    if (!glfwInit())
	{
		printf("failed init\n");
        return;
	}

    /* Create a windowed mode window and its OpenGL context */
    window = glfwCreateWindow(width, height, name, NULL, NULL);
    if (!window)
    {
		printf("no window\n");
        glfwTerminate();
        return;
    }
	
    /* Make the window's context current */
    glfwMakeContextCurrent(window);
	glViewport(0,0,width,height);
    glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
	
	GLenum err = glewInit();
	if (GLEW_OK != err)
	{
		printf("GLEW init failed: %s\n", glewGetErrorString(err));
		return;
	}
	
	glEnable(GL_DEBUG_OUTPUT);
	glDebugMessageCallback(MessageCallback, 0);
	
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	
	defaultShader = create_shader(&DEFAULT_FRAGMENT_SHADER, &DEFAULT_VERTEX_SHADER);
}

static unsigned int create_shader(glsl_shader* fragment, glsl_shader* vertex)
{
	int result;
	int  success;
	char infoLog[512];
	FILE* fp;
	unsigned int vertex_shader = glCreateShader(GL_VERTEX_SHADER);
	unsigned int fragment_shader = glCreateShader(GL_FRAGMENT_SHADER);
	
	glShaderSource(vertex_shader, 1, &vertex->text, 0);
	glCompileShader(vertex_shader);
	glGetShaderiv(vertex_shader, GL_COMPILE_STATUS, &success);
	if(!success)
	{
		glGetShaderInfoLog(vertex_shader, 512, 0, infoLog);
		printf("ERROR::SHADER::VERTEX::COMPILATION_FAILED\n%s\n", infoLog);
	}

	glShaderSource(fragment_shader, 1, &fragment->text, 0);
	glCompileShader(fragment_shader);
	glGetShaderiv(fragment_shader, GL_COMPILE_STATUS, &success);
	if(!success)
	{
		glGetShaderInfoLog(fragment_shader, 512, 0, infoLog);
		printf("ERROR::SHADER::FRAGMENT::COMPILATION_FAILED\n%s\n", infoLog);
	}
	
	unsigned int program = glCreateProgram();
    glAttachShader(program, vertex_shader);
    glAttachShader(program, fragment_shader);
    glLinkProgram(program);
	glGetProgramiv(program, GL_LINK_STATUS, &success);
	if(!success)
	{
		glGetProgramInfoLog(program, 512, 0, infoLog);
		printf("ERROR::PROGRAM::SHADER::COMPILATION_FAILED\n%s\n", infoLog);
	}
	glDeleteShader(vertex_shader);
	glDeleteShader(fragment_shader);
	return program;
}

void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	float new_ar = width/(float)height;
	int new_w = width;
	int new_h = height;
	int x = 0;
	int y = 0;
	
	if(new_ar > aspect_ratio)
	{
		new_w = (int)(aspect_ratio*height);
		x = (width-new_w)/2;
	}
	else if(new_ar < aspect_ratio)
	{
		new_h = (int)(width/aspect_ratio);
		y = (height-new_h)/2;
	}
	
	glViewport(x,y,new_w,new_h);
}