#ifndef OPENAL_ENTRY_HPP
#define OPENAL_ENTRY_HPP

#include "resource.hpp"
#include <AL/al.h>
#include <AL/alc.h>
#include <AL/alext.h>

extern unsigned int* audio_buffers;
void openal_init();
void CloseAL();
unsigned int LoadSound(const wav_audio* wav);

#endif