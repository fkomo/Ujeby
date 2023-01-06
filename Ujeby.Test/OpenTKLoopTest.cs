using OpenTK.Graphics.OpenGL4;
using Ujeby.Graphics.OpenTK;
using Ujeby.Vectors;

namespace Ujeby.Test
{
	internal class OpenTKLoopTest : OpenTKLoop
	{
		public override string Name => nameof(OpenTKLoopTest);

		private readonly float[] _vertices =
		{
			-0.5f, -0.5f, 0.0f, // Bottom-left vertex
             0.5f, -0.5f, 0.0f, // Bottom-right vertex
             0.0f,  0.5f, 0.0f  // Top vertex
        };

		// What these objects are will be explained in OnLoad.
		private int _vertexBufferObject;

		private int _vertexArrayObject;

		// This class is a wrapper around a shader, which helps us manage it.
		// The shader class's code is in the Common project.
		// What shaders are and what they're used for will be explained later in this tutorial.
		private Shader _shader;

		public OpenTKLoopTest(v2i windowSize, string title) : base(windowSize, title)
		{
		}

		protected override void Init()
		{
			// We need to send our vertices over to the graphics card so OpenGL can use them.
			// To do this, we need to create what's called a Vertex Buffer Object (VBO).
			// These allow you to upload a bunch of data to a buffer, and send the buffer to the graphics card.
			// This effectively sends all the vertices at the same time.

			// First, we need to create a buffer. This function returns a handle to it, but as of right now, it's empty.
			_vertexBufferObject = GL.GenBuffer();

			// Now, bind the buffer. OpenGL uses one global state, so after calling this,
			// all future calls that modify the VBO will be applied to this buffer until another buffer is bound instead.
			// The first argument is an enum, specifying what type of buffer we're binding. A VBO is an ArrayBuffer.
			// There are multiple types of buffers, but for now, only the VBO is necessary.
			// The second argument is the handle to our buffer.
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

			// Finally, upload the vertices to the buffer.
			// Arguments:
			//   Which buffer the data should be sent to.
			//   How much data is being sent, in bytes. You can generally set this to the length of your array, multiplied by sizeof(array type).
			//   The vertices themselves.
			//   How the buffer will be used, so that OpenGL can write the data to the proper memory space on the GPU.
			//   There are three different BufferUsageHints for drawing:
			//     StaticDraw: This buffer will rarely, if ever, update after being initially uploaded.
			//     DynamicDraw: This buffer will change frequently after being initially uploaded.
			//     StreamDraw: This buffer will change on every frame.
			//   Writing to the proper memory space is important! Generally, you'll only want StaticDraw,
			//   but be sure to use the right one for your use case.
			GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

			// One notable thing about the buffer we just loaded data into is that it doesn't have any structure to it. It's just a bunch of floats (which are actaully just bytes).
			// The opengl driver doesn't know how this data should be interpreted or how it should be divided up into vertices. To do this opengl introduces the idea of a 
			// Vertex Array Obejct (VAO) which has the job of keeping track of what parts or what buffers correspond to what data. In this example we want to set our VAO up so that 
			// it tells opengl that we want to interpret 12 bytes as 3 floats and divide the buffer into vertices using that.
			// To do this we generate and bind a VAO (which looks deceptivly similar to creating and binding a VBO, but they are different!).
			_vertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(_vertexArrayObject);

			// Now, we need to setup how the vertex shader will interpret the VBO data; you can send almost any C datatype (and a few non-C ones too) to it.
			// While this makes them incredibly flexible, it means we have to specify how that data will be mapped to the shader's input variables.

			// To do this, we use the GL.VertexAttribPointer function
			// This function has two jobs, to tell opengl about the format of the data, but also to associate the current array buffer with the VAO.
			// This means that after this call, we have setup this attribute to source data from the current array buffer and interpret it in the way we specified.
			// Arguments:
			//   Location of the input variable in the shader. the layout(location = 0) line in the vertex shader explicitly sets it to 0.
			//   How many elements will be sent to the variable. In this case, 3 floats for every vertex.
			//   The data type of the elements set, in this case float.
			//   Whether or not the data should be converted to normalized device coordinates. In this case, false, because that's already done.
			//   The stride; this is how many bytes are between the last element of one vertex and the first element of the next. 3 * sizeof(float) in this case.
			//   The offset; this is how many bytes it should skip to find the first element of the first vertex. 0 as of right now.
			// Stride and Offset are just sort of glossed over for now, but when we get into texture coordinates they'll be shown in better detail.
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

			// Enable variable 0 in the shader.
			GL.EnableVertexAttribArray(0);

			// We've got the vertices done, but how exactly should this be converted to pixels for the final image?
			// Modern OpenGL makes this pipeline very free, giving us a lot of freedom on how vertices are turned to pixels.
			// The drawback is that we actually need two more programs for this! These are called "shaders".
			// Shaders are tiny programs that live on the GPU. OpenGL uses them to handle the vertex-to-pixel pipeline.
			// Check out the Shader class in Common to see how we create our shaders, as well as a more in-depth explanation of how shaders work.
			// shader.vert and shader.frag contain the actual shader code.
			_shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

			// Now, enable the shader.
			// Just like the VBO, this is global, so every function that uses a shader will modify this one until a new one is bound instead.
			_shader.Use();

			// Setup is now complete! Now we move to the OnRenderFrame function to finally draw the triangle.
		}

		protected override void Update()
		{
		}

		protected override void Render()
		{
			// To draw an object in OpenGL, it's typically as simple as binding your shader,
			// setting shader uniforms (not done here, will be shown in a future tutorial)
			// binding the VAO,
			// and then calling an OpenGL function to render.

			// Bind the shader
			_shader.Use();

			// Bind the VAO
			GL.BindVertexArray(_vertexArrayObject);

			// And then call our drawing function.
			// For this tutorial, we'll use GL.DrawArrays, which is a very simple rendering function.
			// Arguments:
			//   Primitive type; What sort of geometric primitive the vertices represent.
			//     OpenGL used to support many different primitive types, but almost all of the ones still supported
			//     is some variant of a triangle. Since we just want a single triangle, we use Triangles.
			//   Starting index; this is just the start of the data you want to draw. 0 here.
			//   How many vertices you want to draw. 3 for a triangle.
			GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

			// OpenTK windows are what's known as "double-buffered". In essence, the window manages two buffers.
			// One is rendered to while the other is currently displayed by the window.
			// This avoids screen tearing, a visual artifact that can happen if the buffer is modified while being displayed.
			// After drawing, call this function to swap the buffers. If you don't, it won't display what you've rendered.
			SwapBuffers();

			// And that's all you have to do for rendering! You should now see a yellow triangle on a black screen.
		}

		protected override void Destroy()
		{
			// Unbind all the resources by binding the targets to 0/null.
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.UseProgram(0);

			// Delete all the resources.
			GL.DeleteBuffer(_vertexBufferObject);
			GL.DeleteVertexArray(_vertexArrayObject);

			GL.DeleteProgram(_shader.Handle);
		}

		protected override void LeftMouseDown()
		{

		}

		protected override void LeftMouseUp()
		{

		}
	}
}
