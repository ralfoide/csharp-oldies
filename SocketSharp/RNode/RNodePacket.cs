using System;

namespace RNode
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class RNodePacket
	{
		#region private fields

		private ulong mSignHead;
		private ulong mSignApp;

		#endregion

		public RNodePacket()
		{
			mSignHead = 't';
		}

		/// <summary>
		/// Returns the signature for the header of the packet
		/// </summary>
		public ulong HeaderSignature
		{
			get
			{
				return mSignHead;
			}
		}

	
		/// <summary>
		/// Returns the signature for the header of the packet
		/// </summary>
		public ulong AppSignature
		{
			get
			{
				return mSignApp;
			}
			set
			{
				mSignApp = value;
			}
		}
	}
}
