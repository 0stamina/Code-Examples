#include "openal_entry.hpp"
#include <stdio.h>
#include <limits.h>

ALuint* audio_buffers;
void openal_init()
{
	ALCdevice* device;
	ALCcontext* context;
	ALenum error;
	device = alcOpenDevice(alcGetString(NULL, ALC_DEFAULT_DEVICE_SPECIFIER)); // select the "preferred device"
	
	if (device)
	{
		context = alcCreateContext(device,NULL);
		alcMakeContextCurrent(context);
	}
}

void CloseAL()
{
    ALCdevice *device;
    ALCcontext *ctx;

    ctx = alcGetCurrentContext();
    if(ctx == NULL)
        return;

    device = alcGetContextsDevice(ctx);

    alcMakeContextCurrent(NULL);
    alcDestroyContext(ctx);
    alcCloseDevice(device);
}

ALuint LoadSound(const wav_audio* wav)
{
    ALenum err, format;
    ALuint buffer;
	/* Get the sound format, and figure out the OpenAL format */
    format = AL_NONE;
    if(wav->num_channels == 1)
        format = AL_FORMAT_MONO16;
    else if(wav->num_channels == 2)
        format = AL_FORMAT_STEREO16;
    else if(wav->num_channels == 3)
        format = AL_FORMAT_BFORMAT2D_16;
    else if(wav->num_channels == 4)
        format = AL_FORMAT_BFORMAT3D_16;
    if(!format)
    {
        fprintf(stderr, "Unsupported channel count: %d\n", wav->num_channels);
        return 0;
    }

    /* Buffer the audio data into a new buffer object, then free the data and
     * close the file.
     */
    buffer = 0;
    alGenBuffers(1, &buffer);
    alBufferData(buffer, format, wav->data, (ALsizei)wav->siz, wav->sample_rate);

    /* Check if an error occured, and clean up if so. */
    err = alGetError();
    if(err != AL_NO_ERROR)
    {
        fprintf(stderr, "OpenAL Error: %s\n", alGetString(err));
        if(buffer && alIsBuffer(buffer))
            alDeleteBuffers(1, &buffer);
        return 0;
    }

    return buffer;
}