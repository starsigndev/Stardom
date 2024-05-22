#version 330 core

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 vP;
layout(location = 1) in vec4 vCol;
layout(location = 2) in vec2 vUV;
layout(location = 3) in vec4 vExt;

uniform mat4 se_Projection;

// Output data ; will be interpolated for each fragment.
out vec2 UV;
out vec4 col;
out vec4 ext;

void main(){

	UV = vUV;
	col = vCol;
	ext = vExt;
	gl_Position = se_Projection * vec4(vP,1);
	
}