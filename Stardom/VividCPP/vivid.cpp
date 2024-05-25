#include "vivid.h"
#include <stdio.h>
#define STB_IMAGE_IMPLEMENTATION
#include "stb_image.h"
int initVivid(int a) {

	printf("Vivid Video22 initialized.\n");
	printf("Val:%d\n", a);
	return 0;

}

VideoDecoder* createDecoder() {

	return new VideoDecoder;

}

void playVideo(VideoDecoder* dec,const char* path) {

	printf("Playing:");
	printf(path);
	printf("|\n");
	dec->OpenVideo(path);




}

void updateDecoder(VideoDecoder* dec) {

	dec->UpdateDecoder();

}

VideoFrame* vgetFrame(VideoDecoder* decoder) {

	return decoder->GetImage();

}

int vgetFrameWidth(VideoFrame* f) {

	return f->W;

}

int vgetFrameHeight(VideoFrame* f) {

	return f->H;

}

void* vgetFramebuf(VideoFrame* f) {

	return f->data;

}

bool visDone(VideoDecoder* dec) {

	return dec->IsVideoDone();

}

int img_w, img_h, img_channels;
void* img_buf;

void vLoadImg(const char* path)
{
	img_buf = (void*)stbi_load(path, &img_w, &img_h, &img_channels, 0);


}

int vImgW()
{
	return img_w;
}
int vImgH() {

	return img_h;

}
void* vImgBuf() {

	return img_buf;

 }
int vImgChannels() {

	return img_channels;

 }

void vFreeImg(void * img) {

	stbi_image_free(img_buf);

}