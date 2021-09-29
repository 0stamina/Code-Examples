#ifndef GAME_STATE_HPP
#define GAME_STATE_HPP

#include "opengl_entry.hpp"
#include "openal_entry.hpp"
#include "global_vals.hpp"
#include "actor.hpp"

int state = FIELD_STATE;
bool state_active = false;

void field_init();
void field_draw();
void field_run();
//void field_init();
//void field_draw();
//void field_run();

void init()
{
	switch(state)
	{
		case FIELD_STATE:
		{
			field_init();
			break;
		}
	}
	state_active = true;
}

void draw()
{
	switch(state)
	{
		case FIELD_STATE:
		{
			field_draw();
			break;
		}
	}
}

void run()
{
	switch(state)
	{
		case FIELD_STATE:
		{
			field_run();
			break;
		}
	}
}


actor field_actors[10];
actor* player;
void field_init()
{
	player = &field_actors[0];
	init_actor(player);
	player->trans = glm::translate(player->trans, glm::vec3(0.15,0.6,0));
}

void field_draw()
{
	glUseProgram(defaultShader);
	for(int i = 9; i >= 0; i--)
	{
		if(field_actors[i].exists)
		{
			glUniformMatrix4fv(glGetUniformLocation(defaultShader, "transform"), 1, GL_FALSE, glm::value_ptr(field_actors[i].trans));
			glBindVertexArray(field_actors[i].spr_buf.VAO);
			glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
		}
	}
}

void field_run()
{
	glm::vec3 dir = glm::vec3(0,0,0);
	float speed = 0.4/TARGET_FPS;
	if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
        dir.y += speed;
    if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
        dir.y += -speed;
    if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
        dir.x += -speed;
    if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
        dir.x += speed;
	player->trans = glm::translate(player->trans, dir);
}

#endif