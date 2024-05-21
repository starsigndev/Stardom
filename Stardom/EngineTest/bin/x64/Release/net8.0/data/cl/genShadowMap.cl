// Define the constant value for pi
#define M_PI 3.14159265358979323846
#define DEG_TO_RAD (M_PI / 180.0f)

// Function to convert degrees to radians
float degreesToRadians(float degrees) {
    return degrees * DEG_TO_RAD;
}

float2 RotateAndScale(float2 vector, float rotate, float scale) {
    // Convert the rotation angle from degrees to radians
    float radians = degreesToRadians(rotate);

    // Calculate the sine and cosine of the rotation angle
    float c = cos(radians);
    float s = sin(radians);

    // Apply the rotation
    float xNew = vector.x * c - vector.y * s;
    float yNew = vector.x * s + vector.y * c;

    // Apply the scaling
    xNew *= scale;
    yNew *= scale;

    // Return the transformed vector
    return (float2)(xNew, yNew);
}

uchar getPixel(__global uchar* spriteBuffer, int px, int py, int spriteWidth, int spriteHeight, float spriteRotation) {
    float mx = spriteWidth / 2.0f;
    float my = spriteHeight / 2.0f;

    float nx = px - mx;
    float ny = py - my;

    float2 pos = (float2)(nx, ny);

    float2 sv = RotateAndScale(pos, spriteRotation, 1.0f);

    nx = mx + sv.x;
    ny = my + sv.y;

    int ix = (int)nx;
    int iy = (int)ny;


    if (ix < 0 || ix >= spriteWidth || iy < 0 || iy >= spriteHeight) {
        return 0; // Return a default value for out-of-bounds access
    }

    int index = iy * spriteWidth + ix;
    return spriteBuffer[index];
}

__kernel void generateShadows(
    __global float2* lightPos,
    __global float2* spritePos,
    int spriteCount,
    __global uchar* sprites,
    __global int* spriteIndex,
    __global float2* spriteSize,
    __global float* spriteRotations,
    float lightRange,
    __global float* close
) {
    int id = get_global_id(0);
    float i = (float)id;

    float sx = lightPos[0].x;
    float sy = lightPos[0].y;

    float xi = cos(degreesToRadians(i + 180));
    float yi = sin(degreesToRadians(i + 180));

    float dx = sx + xi * lightRange;
    float dy = sy + yi * lightRange;

    float lxd = dx - sx;
    float lyd = dy - sy;

    float steps = max(fabs(lxd), fabs(lyd));

    float mxi = (lxd / steps)*4;
    float myi = (lyd / steps)*4;
    steps = steps /4;

    float cx = sx;
    float cy = sy;

    float cd = 1.0f;

    for (int li = 0; li < steps; li++) {

        for(int ii=0;ii<spriteCount;ii++){
        
        int spriteWidth = spriteSize[ii].x;
        int spriteHeight = spriteSize[ii].y;
        int base = spriteIndex[ii];

        if (cx > spritePos[ii].x-spriteWidth/2.0 && cx < spritePos[ii].x + spriteWidth/2.0) {
          //  cd = 0.8f;
            if (cy > spritePos[ii].y-spriteHeight/2 && cy < spritePos[ii].y + spriteHeight/2.0) {
         

            float mx = spriteSize[ii].x/2;
            float my = spriteSize[ii].y/2;

             int ix = (int)cx-(int)(spritePos[ii].x-spriteWidth/2);
            int iy = (int)cy-(int)(spritePos[ii].y-spriteHeight/2);


            float nx = ix-mx;
            float ny = iy-my;


            float2 sp = (float2)(nx, ny);


            float2 np = RotateAndScale(sp,-spriteRotations[ii],1.0);

            nx = mx + np.x;
            ny = my + np.y;

            //nx = (float)ix;
            //ny = (float)iy;

            if(nx>=0.0 && nx<spriteSize[ii].x)
            {
                if(ny>=0.0 && ny<spriteSize[ii].y)
                {

            int loc = (int)ny*(int)spriteSize[ii].x+(int)nx;
            
            uchar pix = sprites[base+loc];

              //  cd=0.5;

             //   uchar pix = getPixel(spriteBuffer, (int)rx, (int)ry, imageWidth, imageHeight, spriteRotation);
               // if (pix > 10) {
                  // cd = 0.1f;
                  if(pix>0){
                    float cdx = cx - sx;
                    float cdy = cy - sy;
                    float ad = sqrt(cdx * cdx + cdy * cdy) / lightRange;
                    cd = min(cd, ad);
                //}
                  }
                }
            }
            }
        }
        }
        cx += mxi;
        cy += myi;
    }

    close[id] = cd;
}