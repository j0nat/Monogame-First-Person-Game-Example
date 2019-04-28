#if OPENGL
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;

float3 LightVector;
float4 LightColor;
float LightPower;

texture Texture;
SamplerState TextureSampler
{
	texture = <Texture>;
    MinFilter = linear;
    MagFilter = Anisotropic;
    AddressU = Wrap;
    AddressV = Wrap;
};


struct MyVertexInput
{
	float4 position : POSITION;
	float2 texcoord : TEXCOORD0;
	float4 normal : NORMAL;
};

struct MyVertexOutput
{
	float4 position : POSITION;
	float2 texcoord : TEXCOORD0;
	float4 color : COLOR0;
};

MyVertexOutput VertexShaderFunction( MyVertexInput input )
{
	MyVertexOutput output = (MyVertexOutput)0;
	//transform
	float4x4 viewProj = mul(View,Projection);
	float4x4 WorldViewProj = mul(World,viewProj);
	output.position = mul(input.position, WorldViewProj);
	//lighting
	float4 normal = mul( input.normal, WorldInverseTranspose );
	float intensity = dot( (float3)normal, (float3)LightVector );
	output.color = saturate( LightColor * LightPower * intensity );
	output.texcoord = input.texcoord;
	return output;
}

float4 PixelShaderFunction(MyVertexOutput input) : COLOR0
{
	float4 light = saturate( input.color + LightColor * LightPower );
	return ( tex2D( TextureSampler, input.texcoord ) * light );
}

technique BasicTextured
{
	pass P0
	{
		vertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		pixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}