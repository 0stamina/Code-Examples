#ifndef ACTOR_HPP
#define ACTOR_HPP

#include "opengl_entry.hpp"
#include "openal_entry.hpp"
#include "global_vals.hpp"
#include <stdio.h>

struct actor
{
	bool exists = false;
	glm::mat4 trans;
	sprite_buffers spr_buf;
	unsigned int snd_bufs[10];
};

void init_actor(actor* ob)
{
	float vertices[] = {
        -0.8f, -0.4f, 0.0f,  // top right
        -0.8f, -1.0f, 0.0f,  // bottom right
        -1.0f, -1.0f, 0.0f,  // bottom left
        -1.0f, - 0.4f, 0.0f   // top left 
    };
	glGenVertexArrays(1, &ob->spr_buf.VAO);
    glGenBuffers(1, &ob->spr_buf.VBO);
    glGenBuffers(1, &ob->spr_buf.EBO);
	
    glBindVertexArray(ob->spr_buf.VAO);
    glBindBuffer(GL_ARRAY_BUFFER, ob->spr_buf.VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ob->spr_buf.EBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
	
    glEnableVertexAttribArray(0);
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindVertexArray(0);
	
	ob->trans = glm::mat4(1.0f);
	
	ob->exists = true;
}

void destroy_actor(actor* ob)
{
	glDeleteVertexArrays(1, &ob->spr_buf.VAO);
    glDeleteBuffers(1, &ob->spr_buf.VBO);
    glDeleteBuffers(1, &ob->spr_buf.EBO);
}

#endif
