
#ifndef TILEDMAPCHUNKER_H
#define TILEDMAPCHUNKER_H

#include <fstream>
#include <vector>
#include <map>
#include <cppfs/FileHandle.h>
#include <cppfs/FilePath.h>
#include <string>

using namespace std;
using namespace cppfs;
using namespace cppfs::fs;

class Chunker
{

	streampos pos;
	vector<streampos> layers;
	vector<pair<map<pair<double, double>, streampos>, streampos>> objectGroups;

	string xmlHeader;

	int baseWidth;
	int baseHeight;

	int tileWidth;
	int tileHeight;

	int chunkWidth;
	int chunkHeight;

public:
	ifstream inputfile;
	string inputdir;
	string inputfile_name;

	ofstream outputfile;
	string outuptdir;

	Chunker(string = "", string = "");
	void Run();
	void ChunkSize(int , int);
};

Chunker::Chunker(string inputdir, string outuptdir)
{
	pos = 0;
	this->inputdir = inputdir;
	if (inputdir != "")
	{
		inputfile.open(inputdir);
		FilePath path(inputdir);
		inputfile_name = path.baseName();
	}

	this->outuptdir = outuptdir;
}

void Chunker::ChunkSize(int w, int h)
{
	chunkWidth = w;
	chunkHeight = h;
}

void Chunker::Run()
{
	if (chunkWidth <= 0)
	{
		printf("Chunk width must be set to a value above 0.\n");
	}
	
	if (chunkHeight <= 0)
	{
		printf("Chunk height must be set to a value above 0.\n");
	}

	if (inputdir == "")
	{
		printf("Input file must be set\n");
	}

	if (!inputfile.good())
	{
		printf("Input file is unreadable\n");
		return;
	}

	string in;

	{
		getline(inputfile, in);
		xmlHeader += (in + '\n');
		getline(inputfile, in);
		size_t p = in.find("width=") + 7;
		baseWidth = stoi(in.substr(p, in.find_first_of('\"', p)));
		p = in.find("height=", p) + 8;
		baseHeight = stoi(in.substr(p, in.find_first_of('\"', p)));
		p = in.find("tilewidth=", p) + 11;
		tileWidth = stoi(in.substr(p, in.find_first_of('\"', p)));
		p = in.find("tileheight=", p) + 12;
		tileHeight = stoi(in.substr(p, in.find_first_of('\"', p)));
		xmlHeader += (in + '\n');
	}

	if (!inputfile.good())
	{
		printf("Input file is unreadable\n");
		return;
	}

	while(inputfile.good())
	{
		pos = inputfile.tellg();
		getline(inputfile, in);
		if(in.find("<layer") != in.npos)
		{
			layers.push_back(pos);
			break;
		}
		xmlHeader += (in + '\n');
	}

	if (!inputfile.good())
	{
		printf("Input file is unreadable\n");
		return;
	}

	while (inputfile.good())
	{

		pos = inputfile.tellg();
		getline(inputfile, in);
		if (in.find("<data") != in.npos)
		{
			for (int i = 0; i < baseHeight; i++)
			{
				inputfile.ignore(1000, '\n');
			}
		}
		if (in.find("<layer") != in.npos)
		{
			layers.push_back(pos);
		}

		if (in.find("<objectgroup") != in.npos)
		{
			long long p = pos;
			objectGroups.emplace_back(pair<map<pair<double, double>, streampos>, streampos>(map <pair<double, double>, streampos>(), p));
			while (inputfile.good())
			{
				pos = inputfile.tellg();
				getline(inputfile, in);
				if (in.find("<object") != in.npos)
				{
					size_t p = in.find("x=") + 3;
					double x = stod(in.substr(p, in.find_first_of('\"', p)));
					p = in.find("y=", p) + 3;
					double y = stod(in.substr(p, in.find_first_of('\"', p)));
					objectGroups[objectGroups.size() - 1].first.insert(pair<pair<double, double>, streampos>(pair<double, double>(x, y), pos));
				}
				if (in.find("</objectgroup") != in.npos)
				{
					break;
				}
			}
		}
	}

	int numChunksX = baseWidth % chunkWidth == 0 ? baseWidth / chunkWidth : (baseWidth / chunkWidth) + 1;
	int numChunksY = baseHeight % chunkHeight == 0 ? baseHeight / chunkHeight : (baseHeight / chunkHeight) + 1;

	for (int chunkY = 0; chunkY < numChunksY; chunkY++)
	{
		for (int chunkX = 0; chunkX < numChunksX; chunkX++)
		{
			inputfile.clear();

			int sizeX = baseWidth / chunkWidth > chunkX ? chunkWidth : baseWidth % chunkWidth;
			int sizeY = baseHeight / chunkHeight > chunkY ? chunkHeight : baseHeight % chunkHeight;

			inputfile.seekg(0, inputfile.beg);

			string filedir = inputfile_name + '_' + to_string(chunkX) + '_' + to_string(chunkY) + ".tmx";
			printf("Creating \"%s\"\n", filedir.c_str());

			FileHandle file = open(filedir);
			FileHandle dir = open(outuptdir);


			file.move(dir);

			outputfile.open(dir.path() + file.fileName());
			if (!outputfile.good())
			{
				printf("FATAL_ERROR\n");
			}
			
			{
				string newXmlHeader = xmlHeader;

				size_t p = newXmlHeader.find("width=") + 7;
				newXmlHeader.replace(p, newXmlHeader.find_first_of('\"', p) - p, to_string(sizeX));

				p = newXmlHeader.find("height=", p) + 8;
				newXmlHeader.replace(p, newXmlHeader.find_first_of('\"', p) - p, to_string(sizeY));
				outputfile << newXmlHeader << endl;
			}



			for (size_t i = 0; i < layers.size(); i++)
			{
				inputfile.seekg(layers[i], inputfile.beg);
				while (inputfile.good())
				{
					getline(inputfile, in);

					if (in.find("<layer") != in.npos)
					{
						size_t p = in.find("width=") + 7;
						in.replace(p, in.find_first_of('\"', p) - p, to_string(sizeX));

						p = in.find("height=", p) + 8;
						in.replace(p, in.find_first_of('\"', p) - p, to_string(sizeY));
					}

					outputfile << in << endl;

					if (in.find("<data") != in.npos)
					{
						inputfile.clear();
						break;
					}
				}

				for (int u = 0; u < chunkY * chunkHeight; u++)
				{
					inputfile.ignore(1000, '\n');
				}

				for (int u = 0; u < sizeY; u++)
				{
					for (int j = 0; j < chunkX * chunkWidth; j++)
					{
						inputfile.ignore(1000, ',');
					}

					getline(inputfile, in);

					{
						int n = 0;
						size_t j = 0;
						while (j < in.size() && n < sizeX)
						{
							if (in[j] == ',')
							{
								n++;
							}
							j++;
						}
						if (j < in.size())
						{
							in.erase(j, in.npos);
						}
					}

					if (u == sizeY - 1)
					{
						if (in.back() == ',')
						{
							in.pop_back();
						}
					}
					outputfile << in << endl;
				}
				outputfile << "</data>\n </layer>\n";
			}

			for (size_t i = 0; i < objectGroups.size(); i++)
			{
				inputfile.clear();
				inputfile.seekg(objectGroups[i].second);
				while (inputfile.good())
				{
					getline(inputfile, in);
					if (in.find("<object id") != in.npos)
					{
						break;
					}
					outputfile << in << endl;
				}
				for (map<pair<double, double>, streampos>::iterator it = objectGroups[i].first.begin(); it != objectGroups[i].first.end(); ++it)
				{
					inputfile.clear();
					if (it->first.first > (double)tileWidth * chunkX * chunkWidth && it->first.second > (double)tileHeight * chunkY * chunkHeight
						&& it->first.first < (double)tileWidth * ((chunkX * (double)chunkWidth) + sizeX + 1) && it->first.second < (double)tileHeight * ((chunkY * (double)chunkHeight) + sizeY + 1))
					{
						double posX = it->first.first - ((double)tileWidth * chunkX * chunkWidth);
						double posY = it->first.second - ((double)tileWidth * chunkY * chunkHeight);
						inputfile.seekg(it->second);
						while (inputfile.good())
						{
							getline(inputfile, in);
							if (in.find("<object") != in.npos)
							{
								size_t p = in.find("x=") + 3;
								in.replace(p, in.find_first_of('\"', p) - p, to_string(posX));
								p = in.find("y=", p) + 3;
								in.replace(p, in.find_first_of('\"', p) - p, to_string(posY));

								if (in.find("/>") != in.npos)
								{
									outputfile << in << endl;
									break;
								}
							}
							outputfile << in << endl;
							if (in.find("</object>") != in.npos)
							{
								break;
							}
						}
					}
				}
				outputfile << " </objectgroup>" << endl;
			}
			outputfile << "</map>";

			inputfile.clear();
			outputfile.close();
		}
	}

	inputfile.close();
}

#endif // !TILEDMAPCHUNKER_H
