/// <summary>
/// TextureAtlasAttachmentLoader.cs
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Attachments
{
	using System;

	public class TextureAtlasAttachmentLoader : IAttachmentLoader {
		private TextureAtlas atlas;
		
		public TextureAtlasAttachmentLoader (TextureAtlas atlas) {
			if (atlas == null) 
			{
				throw new ArgumentException("atlas cannot be null.");
			}

			this.atlas = atlas;
		}
		
		public Attachment newAttachment (AttachmentType attachmentType, String name) {
			Attachment attachment = null;
			switch (attachmentType) {
				case AttachmentType.region:
					attachment = new RegionAttachment(name);
					break;

				case AttachmentType.regionSequence:
					attachment = new RegionSequenceAttachment(name);
					break;

				default:
					throw new ArgumentException("Unknown attachment type: " + attachmentType);
			}
			
			if (attachment is RegionAttachment) {
				// TODO
				/*
				AtlasRegion region = atlas.findRegion(attachment.getName());
				if (region == null)
					throw new RuntimeException("Region not found in atlas: " + attachment + " (" + attachmentType + " attachment: " + name + ")");
				((RegionAttachment)attachment).setRegion(region);
				*/
			}
			
			return attachment;
		}
	}
}