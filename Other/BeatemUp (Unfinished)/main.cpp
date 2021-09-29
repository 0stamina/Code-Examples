#include "opengl_entry.hpp"
#include "openal_entry.hpp"
#include "global_vals.hpp"
#include "game_state.hpp"
#include "actor.hpp"
#include <stdio.h>


int main()
{
	opengl_init(WIDTH, HEIGHT, "Crystal City");
	openal_init();
	
	unsigned int source, buffer;
    float offset;
	
    buffer = LoadSound("sound/OHNO.wav");
    if(!buffer)
    {
        CloseAL();
        return 1;
    }

    /* Create the source to play the sound with. */
    source = 0;
    alGenSources(1, &source);
    alSourcei(source, AL_BUFFER, (ALint)buffer);
    assert(alGetError()==AL_NO_ERROR && "Failed to setup sound source");

    /* Play the sound until it finishes. */
	alSourcef(source, AL_GAIN, 0.35f);
    alSourcePlay(source);
    
	double lasttime = glfwGetTime();
	if(!state_active)
	{
		init();
	}
	
    // Loop until the user closes the window
    while (!glfwWindowShouldClose(window))
    {
        // render
        // ------
		glClearColor(0.04,0.02,0.08,1);
		glClear(GL_COLOR_BUFFER_BIT);
		run();
		draw();
		while (glfwGetTime() < lasttime + 1.0/TARGET_FPS) {
			// TODO: Put the thread to sleep, yield, or simply do nothing
		}
		lasttime += 1.0/TARGET_FPS;
        glfwSwapBuffers(window);
        glfwPollEvents();
    }
    glDeleteProgram(defaultShader);
	
    /* All done. Delete resources, and close down OpenAL. */
    alDeleteSources(1, &source);
    alDeleteBuffers(1, &buffer);

    glfwTerminate();
	CloseAL();
	
    return 0;
}