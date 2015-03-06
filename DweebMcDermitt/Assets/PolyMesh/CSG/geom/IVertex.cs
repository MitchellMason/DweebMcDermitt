namespace ConstructiveSolidGeometry
{

    using UnityEngine;

    public interface IVertex
    {
		Vector3 pos { get; set; }
		float zone { get; set; }
        IVertex clone();
        void flip();
		IVertex interpolate(IVertex other, float t);
    }

}