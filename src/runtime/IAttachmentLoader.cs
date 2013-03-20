/// <summary>
/// AttachmentLoader interface
/// 2013-March
/// </summary>
namespace Spine.Runtime.MonoGame
{
	using System;

	public interface IAttachmentLoader
	{
		/** @return May be null to not load any attachment. */
		Attachment newAttachment (AttachmentType type, String name);
	}
}

