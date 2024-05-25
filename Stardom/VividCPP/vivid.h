#pragma once
#include "VideoDecoder.h"


extern "C"
{

	__declspec(dllexport) int initVivid(int p1);
	__declspec(dllexport) VideoDecoder* createDecoder();
	__declspec(dllexport) void playVideo(VideoDecoder* dec,const char* path);
	__declspec(dllexport) void updateDecoder(VideoDecoder* dec);
	__declspec(dllexport) int vgetFrameWidth(VideoFrame* frame);
	__declspec(dllexport) int vgetFrameHeight(VideoFrame* frame);
	__declspec(dllexport) VideoFrame* vgetFrame(VideoDecoder* dec);
	__declspec(dllexport) void* vgetFramebuf(VideoFrame* frame);
	__declspec(dllexport) bool visDone(VideoDecoder* dec);
	__declspec(dllexport) void vLoadImg(const char* path);
	__declspec(dllexport) int vImgW();
	__declspec(dllexport) int vImgH();
	__declspec(dllexport) void* vImgBuf();
	__declspec(dllexport) int vImgChannels();
	__declspec(dllexport) void vFreeImg(void* img);


}