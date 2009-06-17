//*******************************************************************
/* 

 		Project:	Simple Network Server
 		File:		EntityId.cs

*/ 
//*******************************************************************

using System;
using System.Diagnostics;

//----------------------------
namespace Alfray.SimpleNetworkServer
{
	//---------------------
	public struct EntityId
	{
		public const int kServer = 0;
		public const int kBroadcast = -1;

		//----------------------
		public EntityId(int id)
		{
			if (id < 0)
				throw new ArgumentException("EntityId must be positive or null");
			mId = id;
		}

		//-------------------------------------
		public override bool Equals(object obj)
		{
			return mId.Equals(obj);
		}

		//-------------------------------
		public override string ToString()
		{
			return mId.ToString();
		}

		//-------------------------------
		public override int GetHashCode()
		{
			return mId.GetHashCode();
		}

		//-----------------------------------------------
		public static EntityId operator ++(EntityId id)
		{
			id.mId++;
			return id;
		}

		//-----------------------------------------------
		public static implicit operator int(EntityId id)
		{
			return id.mId;
		}


		//-----------------------------------------------
		public static implicit operator EntityId(int id)
		{
			EntityId v;
			v.mId = id;
			return v;
		}


		//----------------------------------------------------------
		[Conditional("TEST")]
		public static void Test()
		{
			Trace.Write("Test: EntityId\n");

			EntityId id0 = 0;
			Trace.Write("id0: Uninitialized id=0: " + id0.ToString() + "\n");

			id0 = EntityId.kServer;
			Trace.Write("id0: = EntityId.kServer: " + id0.ToString() + "\n");

			id0 = EntityId.kBroadcast;
			Trace.Write("id0: = EntityId.kBroadcast: " + id0.ToString() + "\n");

			EntityId id5 = new EntityId(5);
			Trace.Write("id5: Init(5) id: " + id5.ToString() + "\n");

			Trace.Write("id5: Equals(id0): " + id5.Equals(id0).ToString() + "\n");
			Trace.Write("id5: Equals(5): " + id5.Equals(5).ToString() + "\n");

			id5 = 10;
			Trace.Write("id5: = 10 : " + id5.ToString() + "\n");

			id5++;
			Trace.Write("id5: operator++: " + id5.ToString() + "\n");

			Trace.Write("id5: Cast (int)" + ((int)id5).ToString() + "\n");
		}

		//----------------------------------------------------------
		private int mId;
	}


}


//---------------------------------------------------------------
//
//	$Log: EntityId.cs,v $
//	Revision 1.1  2003/06/10 04:42:30  ralf
//	Conforming to .Net naming rules
//	
//	Revision 1.1.1.1  2003/06/08 18:49:55  ralf
//	Initial revision
//	
//---------------------------------------------------------------

