#version 330 core

in vec2 UV;
in vec4 col;
in vec4 ext;

out vec4 out_color;

uniform sampler2D se_ColorTexture;
uniform sampler2D se_ShadowMap;
uniform vec3 se_LightDiffuse;
uniform vec2 se_LightPosition;
uniform vec2 se_RenderOffset;
uniform vec2 se_RenderSize;
uniform float se_LightRange;
uniform float se_Rotate;

void main(){

    vec3 color = texture(se_ColorTexture,UV).rgb;

    float fc_y = se_RenderSize.y - gl_FragCoord.y;

    float xd = se_LightPosition.x - gl_FragCoord.x;
    float yd = se_LightPosition.y - fc_y;

    float dis = sqrt(xd*xd+yd*yd);

    dis = dis / se_LightRange;

    float la = dis;


    float kx = se_LightPosition.x - gl_FragCoord.x;
    float ky = se_LightPosition.y - fc_y;

    float ka = atan(ky,kx);

    float kd = degrees(ka);

    if (kd< 0.0) {
     kd += 360.0;
    }
    kd = kd + se_Rotate;

    if(kd>360.0f)
    {
        kd = kd -360.0f;
    }
    if(kd<0){
        kd = 360 + kd;
    }

   float sv =1.0f;

    sv=1.0;

    if(ext.x>0){

    vec2 shadow_uv = vec2(kd/360.f,0);


    float ux = shadow_uv.x-0.01;

 

    float samples=0;

    for(float vx=ux;vx<shadow_uv.x+0.01;vx=vx+0.002){

        float close_depth = texture(se_ShadowMap,vec2(vx,0)).r;
        
        if(la>close_depth){

        
        

        }else{

            sv+=1.0f;

        }
        samples++;


    }

    
  sv = sv / samples;

    
    }

//    sv =1.0;

  
    if(dis>1.0) dis = 1.0;

    dis = 1.0 - dis;

    vec4 fcol = vec4(0,0,0,1);

    fcol.rgb = color*dis*sv;

    
    fcol.a = texture(se_ColorTexture,UV).a;

    out_color = fcol;

}