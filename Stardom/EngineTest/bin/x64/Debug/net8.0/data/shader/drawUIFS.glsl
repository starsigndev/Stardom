#version 330 core

in vec2 UV;
in vec4 col;
in vec4 ext;

out vec4 out_color;

uniform sampler2D se_ColorTexture;




void main(){

    vec3 color = texture(se_ColorTexture,UV).rgb;

    vec4 fcol;

    fcol.rgb = color * col.rgb;

    fcol.a = texture(se_ColorTexture,UV).a;

    out_color = fcol;

}