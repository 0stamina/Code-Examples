
#include <iostream>
#include <cstdlib>
#include <cppfs/fs.h>
#include <cppfs/FileHandle.h>
#include <cppfs/FilePath.h>
#include "TiledMapChuncker.h"

using namespace std;
using namespace cppfs;
using namespace cppfs::fs;


int main(int argc, char** argv)
{
	string filename = "";
	string outputdir = "";
	int w = 16;
	int h = 16;

	if (argc == 1)
	{
		printf("MUST DECLARE AN INPUT FILE\n[inTMX] [outPath] [height] [width]");
		return 1;
	}
	if (argc > 5)
	{
		printf("TOO MANY ARGS\n[inTMX] [outPath] [height] [width]");
		return 1;
	}

	Chunker* mainChunker;
	FileHandle fh;
	FilePath path;

	for (int i = 0; i < argc; i++)
	{
		cout << argv[i] << endl;
	}

	switch (argc)
	{
	case 5:
		w = atoi(argv[4]);
	case 4:
		h = atoi(argv[3]);
	case 3:
		outputdir = argv[2];
		if (outputdir == "-")
		{
			outputdir = "";
		}
		else
		{
			fh = open(outputdir);

			if (!fh.isDirectory())
			{
				fh.createDirectory();
			}
			else if(fh.isFile())
			{
				printf("[outPath] must be a folder directory, use - to indicate local directory");
				return 1;
			}

			if (outputdir[outputdir.size() - 1] != '\\')
			{
				outputdir += '\\';
			}
		}
	case 2:
		filename = argv[1];
		fh = open(filename);
		path = filename;
		if (fh.isFile())
		{
			if (path.extension() == " .tmx")
			{
				printf("[inTMX] must be a file of type .tmx\nyou gave a file of type %s", path.extension());
				return 1;
			}
		}
		else if(fh.exists())
		{
			printf("[inTMX] must be a file of type .tmx\nyou gave a directory");
			return 1;
		}
		else
		{
			printf("file does not exist");
			return 1;
		}
		break;
	default:
		break;
	}

	mainChunker = new Chunker(filename, outputdir);

	mainChunker->ChunkSize(w, h);
	mainChunker->Run();
	delete mainChunker;
	
	return 0;
}
