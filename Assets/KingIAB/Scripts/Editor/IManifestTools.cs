using System;
namespace KingKodeStudio.IAB.Editor
{
		public interface IManifestTools
    {
#if UNITY_EDITOR
			void UpdateManifest();
			void ClearManifest();
#endif
		}
}

