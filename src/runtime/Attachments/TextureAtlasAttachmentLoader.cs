/// <summary>
/// TextureAtlasAttachmentLoader.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Attachments
{
	using System;

	using Spine.Runtime.MonoGame.Graphics;

	public class TextureAtlasAttachmentLoader : IAttachmentLoader {
		private TextureAtlas atlas;
		
		public TextureAtlasAttachmentLoader (TextureAtlas atlas) {
			if (atlas == null) 
			{
				throw new ArgumentException("atlas cannot be null.");
			}

			this.atlas = atlas;
		}
		
		public Attachment NewAttachment (AttachmentType attachmentType, String name) {
			switch (attachmentType) {
				case AttachmentType.region:
					return this.CreateRegionAttachment(name);

				case AttachmentType.regionSequence:
					throw new NotImplementedException("Not yet supported by Spine - Future development required");
					
				default:
					throw new ArgumentException("Unknown attachment type: " + attachmentType);
			}
		}

		private RegionAttachment CreateRegionAttachment(String name)
		{
			var attachment = new RegionAttachment(name);

			attachment.setRegion(atlas[name]);

			return attachment;
		}
	}
}