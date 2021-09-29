#ifndef OPENAL_ENTRY_HPP
#define OPENAL_ENTRY_HPP

#include <AL/al.h>
#include <AL/alc.h>
#include <AL/alext.h>
#include <sndfile.h>

extern unsigned int* audio_buffers;
void openal_init();
void CloseAL();
unsigned int LoadSound(const char *filename);

#endif