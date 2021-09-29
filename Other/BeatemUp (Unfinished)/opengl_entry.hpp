#ifndef OPENGL_ENTRY_HPP
#define OPENGL_ENTRY_HPP

#include <stdlib.h>

#define GLEW_STATIC
#include <GL/glew.h>
#include <GL/gl.h>
#include <GL/glext.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

const unsigned int indices[] = 
{
	0, 1, 3,
	1, 2, 3
};
extern unsigned int defaultShader;
extern GLFWwindow* window;
extern float aspect_ratio;

struct sprite_buffers
{
	unsigned int VAO;
	unsigned int VBO;
	unsigned int EBO;
	unsigned int texture_ID;
	unsigned int shader_ID;
	unsigned int* data;
};

struct game_input
{
	
};

void opengl_init(int width, int height, char* name);
unsigned int create_shader(const char* vertex_path, const char* fragment_path);
void framebuffer_size_callback(GLFWwindow* window, int width, int height);
void processInput(GLFWwindow *window);

#endif