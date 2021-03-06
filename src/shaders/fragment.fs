// #version 330 core
// out vec4 FragColor;

// in vec3 ourColor;
// in vec2 TexCoord;

// // texture sampler
// uniform sampler2D texture1;

// void main()
// {
// 	FragColor = texture(texture1, TexCoord).zyxw;
// }


#version 330 core
out vec4 FragColor;

in vec2 TexCoord;

uniform sampler2D screenTexture;
uniform float screenWidth;
uniform float screenHeight;

uniform vec2 resolution;
uniform float zoom;
uniform float cropXLeft;
uniform float cropXRight;
uniform float cropYTop;
uniform float cropYBottom;

#define P(id,a,b,c,d,e,f,g,h) if( id == int(pos.y) ){ int pa = a+2*(b+2*(c+2*(d+2*(e+2*(f+2*(g+2*(h))))))); cha = floor(mod(float(pa)/pow(2.,float(pos.x)-1.),2.)); }

float gray(vec3 _i) {
    return _i.x*0.299+_i.y*0.587+_i.z*0.114;
}

vec4 croppedSample(sampler2D tex, vec2 uv) {
    float xLeftNorm = cropXLeft / screenWidth;
    float xRightNorm = cropXRight / screenWidth;
    float yTopNorm = cropYTop / screenHeight;
    float yBottomNorm = cropYBottom / screenHeight;

    float x = uv.x * (1.0 - xLeftNorm - xRightNorm) + xLeftNorm;
    float y = uv.y * (1.0 - yTopNorm - yBottomNorm) + yBottomNorm;
    return texture(tex, vec2(x, y));
}

void main() {
    vec2 r = vec2(800., 450.) * 2.;
    vec2 uv = vec2(floor(gl_FragCoord.x/8./zoom)*8.*zoom, 1.0 - floor(gl_FragCoord.y/12./zoom)*12.*zoom)/resolution;
    ivec2 pos = ivec2(mod(gl_FragCoord.x/zoom,8.),mod(gl_FragCoord.y/zoom,12.));
    vec4 tex = croppedSample(screenTexture,uv).zyxw;

    float cha = 0.;
    
    float g = gray(tex.xyz);
    // if(true) {
    //     P(11,1,1,1,1,1,1,1,1);
    //     P(10,1,1,1,1,1,1,1,1);
    //     P(9,1,1,1,1,1,1,1,1);
    //     P(8,1,1,1,1,1,1,1,1);
    //     P(7,1,1,1,1,1,1,1,1);
    //     P(6,1,1,1,1,1,1,1,1);
    //     P(5,1,1,1,1,1,1,1,1);
    //     P(4,1,1,1,1,1,1,1,1);
    //     P(3,1,1,1,1,1,1,1,1);
    //     P(2,1,1,1,1,1,1,1,1);
    //     P(1,1,1,1,1,1,1,1,1);
    //     P(0,1,1,1,1,1,1,1,1);
    // }
    if( g < .125 )
    {
        P(11,0,0,0,0,0,0,0,0);
        P(10,0,0,0,0,0,0,0,0);
        P(9,0,0,0,0,0,0,0,0);
        P(8,0,0,0,0,0,0,0,0);
        P(7,0,0,0,0,0,0,0,0);
        P(6,0,0,0,0,0,0,0,0);
        P(5,0,0,0,0,0,0,0,0);
        P(4,0,0,0,0,0,0,0,0);
        P(3,0,0,0,0,0,0,0,0);
        P(2,0,0,0,0,0,0,0,0);
        P(1,0,0,0,0,0,0,0,0);
        P(0,0,0,0,0,0,0,0,0);
    }
    else if( g < .25 ) // .
    {
        P(11,0,0,0,0,0,0,0,0);
        P(10,0,0,0,0,0,0,0,0);
        P(9,0,0,0,0,0,0,0,0);
        P(8,0,0,0,0,0,0,0,0);
        P(7,0,0,0,0,0,0,0,0);
        P(6,0,0,0,0,0,0,0,0);
        P(5,0,0,0,0,0,0,0,0);
        P(4,0,0,0,1,1,0,0,0);
        P(3,0,0,0,1,1,0,0,0);
        P(2,0,0,0,0,0,0,0,0);
        P(1,0,0,0,0,0,0,0,0);
        P(0,0,0,0,0,0,0,0,0);
    }
    else if( g < .375 ) // ,
    {
        P(11,0,0,0,0,0,0,0,0);
        P(10,0,0,0,0,0,0,0,0);
        P(9,0,0,0,0,0,0,0,0);
        P(8,0,0,0,0,0,0,0,0);
        P(7,0,0,0,0,0,0,0,0);
        P(6,0,0,0,0,0,0,0,0);
        P(5,0,0,0,0,0,0,0,0);
        P(4,0,0,0,1,1,0,0,0);
        P(3,0,0,0,1,1,0,0,0);
        P(2,0,0,0,0,1,0,0,0);
        P(1,0,0,0,1,0,0,0,0);
        P(0,0,0,0,0,0,0,0,0);
    }
    else if( g < .5 ) // -
    {
        P(11,0,0,0,0,0,0,0,0);
        P(10,0,0,0,0,0,0,0,0);
        P(9,0,0,0,0,0,0,0,0);
        P(8,0,0,0,0,0,0,0,0);
        P(7,0,0,0,0,0,0,0,0);
        P(6,1,1,1,1,1,1,1,0);
        P(5,0,0,0,0,0,0,0,0);
        P(4,0,0,0,0,0,0,0,0);
        P(3,0,0,0,0,0,0,0,0);
        P(2,0,0,0,0,0,0,0,0);
        P(1,0,0,0,0,0,0,0,0);
        P(0,0,0,0,0,0,0,0,0);
    }
    else if(g < .625 ) // +
    {
        P(11,0,0,0,0,0,0,0,0);
        P(10,0,0,0,0,0,0,0,0);
        P(9,0,0,0,1,0,0,0,0);
        P(8,0,0,0,1,0,0,0,0);
        P(7,0,0,0,1,0,0,0,0);
        P(6,1,1,1,1,1,1,1,0);
        P(5,0,0,0,1,0,0,0,0);
        P(4,0,0,0,1,0,0,0,0);
        P(3,0,0,0,1,0,0,0,0);
        P(2,0,0,0,0,0,0,0,0);
        P(1,0,0,0,0,0,0,0,0);
        P(0,0,0,0,0,0,0,0,0);
    }
    else if(g < .75 ) // *
    {
        P(11,0,0,0,0,0,0,0,0);
        P(10,0,0,0,1,0,0,0,0);
        P(9,1,0,0,1,0,0,1,0);
        P(8,0,1,0,1,0,1,0,0);
        P(7,0,0,1,1,1,0,0,0);
        P(6,0,0,0,1,0,0,0,0);
        P(5,0,0,1,1,1,0,0,0);
        P(4,0,1,0,1,0,1,0,0);
        P(3,1,0,0,1,0,0,1,0);
        P(2,0,0,0,1,0,0,0,0);
        P(1,0,0,0,0,0,0,0,0);
        P(0,0,0,0,0,0,0,0,0);
    }
    else if(g < .875 ) // #
    {
        P(11,0,0,0,0,0,0,0,0);
        P(10,0,0,1,0,0,1,0,0);
        P(9,0,0,1,0,0,1,0,0);
        P(8,1,1,1,1,1,1,1,0);
        P(7,0,0,1,0,0,1,0,0);
        P(6,0,0,1,0,0,1,0,0);
        P(5,0,1,0,0,1,0,0,0);
        P(4,0,1,0,0,1,0,0,0);
        P(3,1,1,1,1,1,1,1,0);
        P(2,0,1,0,0,1,0,0,0);
        P(1,0,1,0,0,1,0,0,0);
        P(0,0,0,0,0,0,0,0,0);
    }
    else // @
    {
        P(11,0,0,0,0,0,0,0,0);
        P(10,0,0,1,1,1,1,0,0);
        P(9,0,1,0,0,0,0,1,0);
        P(8,1,0,0,0,1,1,1,0);
        P(7,1,0,0,1,0,0,1,0);
        P(6,1,0,0,1,0,0,1,0);
        P(5,1,0,0,1,0,0,1,0);
        P(4,1,0,0,1,0,0,1,0);
        P(3,1,0,0,1,1,1,1,0);
        P(2,0,1,0,0,0,0,0,0);
        P(1,0,0,1,1,1,1,1,0);
        P(0,0,0,0,0,0,0,0,0);
    }
    
    vec3 col = tex.xyz/max(tex.x,max(tex.y,tex.z));
    FragColor = vec4(cha*col,1.);
}
