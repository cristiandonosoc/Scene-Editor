/**
 * Toon Vertex Shader
 **/


varying vec3 normal;

void main()
{
	//gl_Position = ftransform();
	//gl_Normal = ftransform(); 
	normal = gl_NormalMatrix*gl_Normal;
	

	gl_Position = ftransform();
}
