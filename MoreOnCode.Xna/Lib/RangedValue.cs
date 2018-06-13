using System;

using Microsoft.Xna.Framework;

namespace MoreOnCode.Lib
{
	// simple class to hold min / max values and generate random values 
	// within those bounds. b ase class uses generics (templates)
	public abstract class RangedValue<T>
	{
		protected RangedValue() : this(default(T), default(T)) {}

		protected RangedValue(T min, T max) : base()
		{
			this.Min = min;
			this.Max = max;
		}

		public T Min { get; set; }
		public T Max { get; set; }
		public T Value { get; set; }

		// random number generator
		protected Random m_rand = new Random();

		// generate a random value between min and max, inclusive
		public abstract T RandomValue();

		// linear interpolation between min and max
		public abstract T Lerp(float progress);
	}

	public class RangedByte : RangedValue<byte>
	{
		public RangedByte() : base() { }
		public RangedByte(byte min, byte max) : base(min, max) { }

		// generate a random value between min and max, inclusive
		public override byte RandomValue()
		{
			Value = (byte)Math.Round(
				MathHelper.Lerp(
					(float)Min,
					(float)Max,
					(float)m_rand.NextDouble()));
			return Value;

		}

		// linear interpolation between min and max
		public override byte Lerp(float progress)
		{
			return (byte)MathHelper.Lerp (Min, Max, progress);
		}
	}

	// type-specific subclass
	public class RangedInt : RangedValue<int>
	{
		public RangedInt() : base() { }
		public RangedInt(int min, int max) : base(min, max) { }

		// generate a random value between min and max, inclusive
		public override int RandomValue()
		{
			Value = (int)Math.Round(
				MathHelper.Lerp(
					(float)Min,
					(float)Max,
					(float)m_rand.NextDouble()));
			return Value;

		}

		// linear interpolation between min and max
		public override int Lerp(float progress)
		{
			return (int)MathHelper.Lerp (Min, Max, progress);
		}
	}

	// type-specific subclass
	public class RangedLong : RangedValue<long>
	{
		public RangedLong() : base() { }
		public RangedLong(long min, long max) : base(min, max) { }

		// generate a random value between min and max, inclusive
		public override long RandomValue()
		{
			Value = (long)Math.Round(
				MathHelper.Lerp(
					(float)Min,
					(float)Max,
					(float)m_rand.NextDouble()));
			return Value;
		}

		// linear interpolation between min and max
		public override long Lerp(float progress)
		{
			return (long)MathHelper.Lerp (Min, Max, progress);
		}
	}

	// type-specific subclass
	public class RangedFloat : RangedValue<float>
	{
		public RangedFloat() : base() { }
		public RangedFloat(float min, float max) : base(min, max) { }

		// generate a random value between min and max, inclusive
		public override float RandomValue()
		{
			// linear interpolation between min and max based on random number
			Value = (float)MathHelper.Lerp(
				(float)Min,
				(float)Max,
				(float)m_rand.NextDouble());
			return Value;

		}

		// linear interpolation between min and max
		public override float Lerp(float progress)
		{
			return MathHelper.Lerp (Min, Max, progress);
		}
	}

	// type-specific subclass
	public class RangedDouble : RangedValue<double>
	{
		public RangedDouble() : base() { }
		public RangedDouble(double min, double max) : base(min, max) { }

		// generate a random value between min and max, inclusive
		public override double RandomValue()
		{
			// linear interpolation between min and max based on random number
			Value = (double)MathHelper.Lerp(
				(float)Min,
				(float)Max,
				(float)m_rand.NextDouble());
			return Value;

		}

		// linear interpolation between min and max
		public override double Lerp(float progress)
		{
			return MathHelper.Lerp ((float)Min, (float)Max, progress);
		}
	}

	// type-specific subclass
	public class RangedVector2 : RangedValue<Vector2>
	{
		public RangedVector2() : base() { }
		public RangedVector2(Vector2 min, Vector2 max) : base(min, max) { }

		// generate a random value between min and max, inclusive
		public override Vector2 RandomValue()
		{
			// linear interpolation between min and max based on random number
			var value = Vector2.Zero;
			value.X = (float)MathHelper.Lerp(
				(float)Min.X,
				(float)Max.X,
				(float)m_rand.NextDouble());
			value.Y = (float)MathHelper.Lerp(
				(float)Min.Y,
				(float)Max.Y,
				(float)m_rand.NextDouble());
			this.Value = value;
			return Value;

		}

		// determine min and max values from a Rectangle
		public static RangedVector2 FromRectangle(Rectangle rect)
		{
			Vector2 v2Min = Vector2.Zero;
			v2Min.X = rect.Left;
			v2Min.Y = rect.Top;

			Vector2 v2Max = Vector2.Zero;
			v2Max.X = rect.Left + rect.Width;
			v2Max.Y = rect.Top + rect.Height;

			return new RangedVector2(v2Min, v2Max);
		}

		// linear interpolation between min and max
		public override Vector2 Lerp(float progress)
		{
			var result = Min;
			result.X = MathHelper.Lerp (Min.X, Max.X, progress);
			result.Y = MathHelper.Lerp (Min.Y, Max.Y, progress);
			return result;
		}
	}

	// type-specific subclass
	public class RangedVector3 : RangedValue<Vector3>
	{
		public RangedVector3() : base() { }
		public RangedVector3(Vector3 min, Vector3 max) : base(min, max) { }

		// generate a random value between min and max, inclusive
		public override Vector3 RandomValue()
		{
			// linear interpolation between min and max based on random number
			var value = Vector3.Zero;
			value.X = (float)MathHelper.Lerp(
				(float)Min.X,
				(float)Max.X,
				(float)m_rand.NextDouble());
			value.Y = (float)MathHelper.Lerp(
				(float)Min.Y,
				(float)Max.Y,
				(float)m_rand.NextDouble());
			value.Z = (float)MathHelper.Lerp(
				(float)Min.Z,
				(float)Max.Z,
				(float)m_rand.NextDouble());
			this.Value = value;
			return Value;

		}

		// linear interpolation between min and max
		public override Vector3 Lerp(float progress)
		{
			var result = Min;
			result.X = MathHelper.Lerp (Min.X, Max.X, progress);
			result.Y = MathHelper.Lerp (Min.Y, Max.Y, progress);
			result.Z = MathHelper.Lerp (Min.Z, Max.Z, progress);
			return result;
		}
	}

	// type-specific subclass
	public class RangedVector4 : RangedValue<Vector4>
	{
		public RangedVector4() : base() { }
		public RangedVector4(Vector4 min, Vector4 max) : base(min, max) { }

		// generate a random value between min and max, inclusive
		public override Vector4 RandomValue()
		{
			// linear interpolation between min and max based on random number
			var value = Vector4.One;
			value.X = (float)MathHelper.Lerp(
				(float)Min.X,
				(float)Max.X,
				(float)m_rand.NextDouble());
			value.Y = (float)MathHelper.Lerp(
				(float)Min.Y,
				(float)Max.Y,
				(float)m_rand.NextDouble());
			value.Z = (float)MathHelper.Lerp(
				(float)Min.Z,
				(float)Max.Z,
				(float)m_rand.NextDouble());
			value.W = (float)MathHelper.Lerp(
				(float)Min.W,
				(float)Max.W,
				(float)m_rand.NextDouble());
			Value = value;
			return Value;
		}

		// determine min and max values from colors
		public static RangedVector4 FromColors(Color min, Color max)
		{
			Vector4 v4Min = Vector4.Zero;
			v4Min.X = (float)min.R / (float)byte.MaxValue;
			v4Min.Y = (float)min.G / (float)byte.MaxValue;
			v4Min.Z = (float)min.B / (float)byte.MaxValue;
			v4Min.W = (float)min.A / (float)byte.MaxValue;

			Vector4 v4Max = Vector4.Zero;
			v4Max.X = (float)max.R / (float)byte.MaxValue;
			v4Max.Y = (float)max.G / (float)byte.MaxValue;
			v4Max.Z = (float)max.B / (float)byte.MaxValue;
			v4Max.W = (float)max.A / (float)byte.MaxValue;

			return new RangedVector4(v4Min, v4Max);
		}

		// determine min and max values from rectangles
		public static RangedVector4 FromRectangles(Rectangle min, Rectangle max)
		{
			Vector4 v4Min = Vector4.Zero;
			v4Min.X = (float)min.X;
			v4Min.Y = (float)min.Y;
			v4Min.Z = (float)min.Width;
			v4Min.W = (float)min.Height;

			Vector4 v4Max = Vector4.Zero;
			v4Max.X = (float)max.X;
			v4Max.Y = (float)max.Y;
			v4Max.Z = (float)max.Width;
			v4Max.W = (float)max.Height;

			return new RangedVector4(v4Min, v4Max);
		}

		// linear interpolation between min and max
		public override Vector4 Lerp(float progress)
		{
			var result = Min;
			result.X = MathHelper.Lerp (Min.X, Max.X, progress);
			result.Y = MathHelper.Lerp (Min.Y, Max.Y, progress);
			result.Z = MathHelper.Lerp (Min.Z, Max.Z, progress);
			result.W = MathHelper.Lerp (Min.W, Max.W, progress);
			return result;
		}

		public static Rectangle AsRectangle(Vector4 value) {
			return new Rectangle (
				(int)Math.Round(value.X),
				(int)Math.Round(value.Y),
				(int)Math.Round(value.Z),
				(int)Math.Round(value.W)
			);
		}

		public static Color AsColor(Vector4 value) {
			return new Color (
				(float)value.X,
				(float)value.Y,
				(float)value.W,
				(float)value.Z
			);
		}
	}
}

