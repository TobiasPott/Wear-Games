using Android.Graphics;
using Android.Views;
using NoXP.Types;
using System;
using System.Globalization;

namespace WearGames
{

    public enum InsetModes
    {
        None,
        Shrink,
        Grow,
    }

    public static class StructExtensions
    {
        public static Color ModifyColor(this Color srcColor, byte clrDecrement, ColorMask mask, Vector3Int colorMaskModifier)
        {
            Color clr = new Color(srcColor.R, srcColor.G, srcColor.B, srcColor.A);
            if (mask.HasFlag(ColorMask.R))
                clr.R = (byte)Math.Clamp(clr.R + clrDecrement * colorMaskModifier.R, 0, 255);
            if (mask.HasFlag(ColorMask.G))
                clr.G = (byte)Math.Clamp(clr.G + clrDecrement * colorMaskModifier.G, 0, 255);
            if (mask.HasFlag(ColorMask.B))
                clr.B = (byte)Math.Clamp(clr.B + clrDecrement * colorMaskModifier.B, 0, 255);
            return clr;
        }


        public static string ToStringAsMinutesAndSeconds(this float time)
        {
            float timeSec = time;
            float timeMin = 0.0f;
            if (time >= 60.0f)
            {
                timeSec = time % 60.0f;
                timeMin = (time - timeSec) / 60.0f;
                return timeMin.ToString("0", CultureInfo.InvariantCulture) + ":" + timeSec.ToString("00.00", CultureInfo.InvariantCulture);
            }
            else
                return timeSec.ToString("00.00", CultureInfo.InvariantCulture);
        }

    }

    public static class Extensions
    {
        private static TransformableVector PositionToWorldSpace = new TransformableVector(0, 0);
        private static TransformableVector PositionToLocalSpace = new TransformableVector(0, 0);

        private static TransformableVector UpVector = new TransformableVector(0, 1);
        private static TransformableVector DownVector = new TransformableVector(0, -1);
        private static TransformableVector LeftVector = new TransformableVector(-1, 0);
        private static TransformableVector RightVector = new TransformableVector(-1, 0);

        private static RectF _colBounds = new RectF(0, 0, 1, 1);
        private static RectF _colBoundsRaw = new RectF(0, 0, 1, 1);
        private static Vector2 _lastIntersectionNormal = new Vector2(0, 1);

        public static Vector2 LastIntersectionNormal
        { get => _lastIntersectionNormal; }


        public static Vector2 GetCollisionNormalForCenterOnUnitCircle(this View view, View other)
        {
            Vector2 center = other.GetCenterPosition();
            return view.GetCollisionNormalForCenterOnUnitCircle(center);
        }
        public static Vector2 GetCollisionNormalForCenterOnUnitCircle(this View view, Vector2 centerPosition)
        {
            return new Vector2(centerPosition.X - view.Width * 0.5f, centerPosition.Y - view.Width * 0.5f).Normalized;
        }



        public static Vector2 GetCenterPosition(this View view)
        {
            if (view != null)
            {
                Vector2 center = view.GetLocalCenter();
                return view.TransformToWorldSpace(center);
            }
            else
                return Vector2.Zero;
        }
        public static Vector2 GetLocalCenter(this View view)
        {
            if (view != null)
                return new Vector2(view.Width * 0.5f, view.Height * 0.5f);
            else
                return Vector2.Zero;
        }

        // GetBoundingCircleRadius does provide the contained bounding circle of a view (not the containing circle)
        public static float GetBoundingCircleRadius(this View view)
        { return MathF.Max(view.Width, view.Height) * 0.5f; }
        // GetInnerCircleRadius does provide the contained bounding circle of the smallest side of a view
        public static float GetInnerCircleRadius(this View view)
        { return MathF.Min(view.Width, view.Height) * 0.5f; }

        public static Vector2 TransformToWorldSpace(this View view)
        { return view.TransformToWorldSpace(); }
        public static Vector2 TransformToWorldSpace(this View view, Vector2 position)
        {
            Matrix worldMatrix = new Matrix();
            view.GetWorldMatrix(worldMatrix);

            PositionToWorldSpace.Set(position);
            // actual perform transformation of the 0,0 origin
            PositionToWorldSpace.TransformAsPoint(worldMatrix);
            return PositionToWorldSpace.Value;
        }

        private static void GetWorldMatrix(this View view, Matrix worldMatrix)
        {
            worldMatrix.PostConcat(view.Matrix);
            if (view.Parent != null && view.Parent is View)
            {
                View parentView = view.Parent as View;
                GetWorldMatrix(parentView, worldMatrix);
            }
        }

        public static Vector2 TransformToLocalSpace(this View view, Vector2 position)
        {
            Matrix worldMatrix = new Matrix();
            view.GetWorldMatrix(worldMatrix);

            PositionToLocalSpace.Set(position);
            // might allow to add all corner points to do world space collision detection
            if (worldMatrix.Invert(worldMatrix))
                PositionToLocalSpace.TransformAsPoint(worldMatrix);
            return PositionToLocalSpace.Value;
        }

        public static Vector2 Up(this View view)
        { return TransformDirection(view, UpVector); }
        public static Vector2 Down(this View view)
        { return TransformDirection(view, DownVector); }
        public static Vector2 Left(this View view)
        { return TransformDirection(view, LeftVector); }
        public static Vector2 Right(this View view)
        { return TransformDirection(view, RightVector); }

        private static Vector2 TransformDirection(View view, TransformableVector transformableVector)
        {
            if (view == null)
                return transformableVector.Value;
            if (view.Parent != null && view.Parent is View)
            {
                Matrix mWorld = new Matrix();
                view.GetWorldMatrix(mWorld);
                transformableVector.TransformAsVector(mWorld);
                return transformableVector.Value;
            }
            else
            {
                transformableVector.TransformAsVector(view.Matrix);
                return transformableVector.Value;
            }
        }


        public static bool IntersectBoundingCircles(this View view, View other, bool inset = false)
        {
            return IntersectBoundingCircles(view.GetCenterPosition(), view.GetBoundingCircleRadius(),
                                            other.GetCenterPosition(), other.GetBoundingCircleRadius(), inset);
        }
        public static bool IntersectBoundingCircles(this View view, Vector2 position, float radius, bool inset = false)
        {
            return IntersectBoundingCircles(view.GetCenterPosition(), view.GetBoundingCircleRadius(),
                                            position, radius, inset);
        }
        public static bool IntersectBoundingCircles(Vector2 circlePosition, float circleRadius, Vector2 position, float radius, bool inset = false)
        {
            float distSqr = (position - circlePosition).LengthSquared();
            if (inset)
                return (MathF.Pow(circleRadius - radius, 2) > distSqr);
            return (MathF.Pow(circleRadius + radius, 2) > distSqr);
        }


        public static bool IntersectBoundsWithCircle(this View view, View other, InsetModes insetMode = InsetModes.None, bool flipCollisionNormal = false)
        {
            return view.IntersectBoundsWithCircle(other.GetCenterPosition(), other.GetBoundingCircleRadius(), insetMode, flipCollisionNormal);
        }
        public static bool IntersectBoundsWithCircle(this View view, Vector2 position, float radius, InsetModes insetMode = InsetModes.None, bool flipCollisionNormal = false)
        {
            _colBounds.Set(view.GetX(), view.GetY(), view.GetX() + view.Width, view.GetY() + view.Height);
            return _colBounds.IntersectBoundsWithCircle(position, radius, insetMode, flipCollisionNormal);
        }
        public static bool IntersectBoundsWithCircle(this IBoundsProvider view, Vector2 position, float radius, InsetModes insetMode = InsetModes.None, bool flipCollisionNormal = false)
        {
            return view.Bounds.IntersectBoundsWithCircle(position, radius, insetMode, flipCollisionNormal);
        }
        public static bool IntersectBoundsWithCircle(this RectF bounds, Vector2 position, float radius, InsetModes insetMode = InsetModes.None, bool flipCollisionNormal = false)
        {
            _colBounds.Set(bounds);
            return IntersectBoundsWithCircle(position, radius, insetMode, flipCollisionNormal);
        }
        private static bool IntersectBoundsWithCircle(Vector2 position, float radius, InsetModes insetMode = InsetModes.None, bool flipCollisionNormal = false)
        {
            _colBoundsRaw.Set(_colBounds);
            if (insetMode == InsetModes.Grow)
                _colBounds.Inset(-radius, -radius);
            else if (insetMode == InsetModes.Shrink)
                _colBounds.Inset(radius, radius);

            Extensions.GetCollisionNormalForBoundsOutside(position, radius);
            return _colBounds.Contains(position.X, position.Y);
        }

        public static void GetCollisionNormalForBoundsOutside(Vector2 centerPosition, float radius = 0)
        {
            if (_colBounds.Contains(_colBoundsRaw))
                GetCollisionNormalForBounds(_colBoundsRaw, _colBounds, centerPosition);
            else
                GetCollisionNormalForBounds(_colBounds, _colBoundsRaw, centerPosition);
        }

        private static void GetCollisionNormalForBounds(RectF inner, RectF outer, Vector2 centerPosition)
        {
            //_lastIntersectionNormal = Vector2.Zero;
            if (centerPosition.X > outer.Left && centerPosition.X < inner.Left)
                _lastIntersectionNormal = Vector2.Left;
            else if (centerPosition.X < outer.Right && centerPosition.X > inner.Right)
                _lastIntersectionNormal = Vector2.Right;

            if (centerPosition.Y > outer.Top && centerPosition.Y < inner.Top)
                _lastIntersectionNormal = Vector2.Up;
            else if (centerPosition.Y < outer.Bottom && centerPosition.Y > inner.Bottom)
                _lastIntersectionNormal = Vector2.Down;
            //_lastIntersectionNormal.Normalize();
        }

    }

}