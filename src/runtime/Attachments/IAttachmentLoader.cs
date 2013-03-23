/// <summary>
/// AttachmentLoader interface
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame.Attachments
{
	using System;

	public interface IAttachmentLoader
	{
		/** @return May be null to not load any attachment. */
		Attachment newAttachment (AttachmentType type, String name);
	}
}

