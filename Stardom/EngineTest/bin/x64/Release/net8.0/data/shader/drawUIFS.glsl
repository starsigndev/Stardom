#version 330 core

in vec2 UV;
in vec4 col;
in vec4 ext;

out vec4 out_color;

uniform sampler2D se_ColorTexture;
uniform sampler2D se_MaskTexture;
uniform sampler2D se_RefractTexture;
uniform float se_BlurFactor;



void main(){

    vec3 color = texture(se_ColorTexture,UV).rgb;

    float blurFactor = ext.x;

    if(blurFactor>0)
    {

        vec3 bcol = vec3(0,0,0);

        float samples=0;

        for(float y=-3;y<3;y++){
            for(float x=-3;x<3;x++){

                vec2 buv = UV;

                buv.x = buv.x + x*blurFactor;
                buv.y = buv.y + y*blurFactor;

                bcol = bcol + texture(se_ColorTexture,buv).rgb;
                samples++;
            }
        }

        bcol = bcol/samples;
        color = bcol;

    }

    vec4 fcol;

    if(ext.z>0)
    {
        //vec2 ruv = UV;

        vec2 ruv = texture(se_RefractTexture,UV).rg;
        ruv = -1.0 + ruv*2.0;
        ruv.x = UV.x + ruv.x*ext.z;
        ruv.y = UV.y + ruv.y*ext.z;

  //      ruv.x += texture(se_RefractTexture,UV).r*ext.z;
//        ruv.y += texture(se_RefractTexture,UV).g*ext.z;
        color = texture(se_ColorTexture,ruv).rgb*2.0;
       
    }

    fcol.rgb = color * col.rgb;

    fcol.a = texture(se_ColorTexture,UV).a* col.a;

    if(ext.y>0)
    {

        fcol.a = texture(se_MaskTexture,UV).a;

        if(fcol.a<=0){

            discard;

        }


    }else{

    

    if(fcol.a<=0.1)
    {
        discard;
        }
    }

    fcol = fcol*col;



    out_color = fcol;

}