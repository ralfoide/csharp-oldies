//*******************************************************************
/* 

 		Project:	Simple Network Server
 		File:		Entities.cs

*/ 
//*******************************************************************

using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

//-------------------------
namespace Alfray.SimpleNetworkServer
{
	//----------------------------------------------------------
	public struct EntityDesc
	{
		public EntityId mId;
		public string mType;
		public string mDesc;
	} // EntityDesc


	//----------------------------------------------------------
	/// <summary>
	/// Summary description for Entities.
	/// </summary>
	public class Entities
	{
		// ---------- Public Constants -------------

		
		// ---------- Public Properties -------------


		// ---------- Public Methods -------------

		//********************
		public Entities()
		{
			mEntities = new Hashtable();
		}



		//-------------------------------------------------------------
		/// <summary>
		/// Gets an instance of an entity.
		/// The caller must lock the entity before modifying it.
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <param name="entity">The out-value (out in C#) of the entity dictionary</param>
		/// <returns>True if the entity exists and was returned</returns>
		//-------------------------------------------------------------
		public bool Get(EntityId id, out IDictionary entity)
		{
			lock(mEntities)
			{
				if (mEntities.Contains(id))
				{
					entity = (IDictionary) mEntities[id];
					return true;
				}
			}

			entity = null;
			return false;
		}


		//-------------------------------------------------------------
		/// <summary>
		/// Adds an entity.
		/// The server adds a key "id" to the entity.
		/// </summary>
		/// <param name="entity">The reference (ref in C#) of the entity dictionary to add</param>
		/// <returns>The server id of the entitiy (>0)</returns>
		//-------------------------------------------------------------
		public int Add(ref IDictionary entity)
		{
			// make sure the default values are present
			// set "desc" and "type" if not set
			if (!entity.Contains("desc")) entity["desc"] = "Unknown";
			if (!entity.Contains("type")) entity["type"] = "unknown";

			lock(mEntities)
			{
				EntityId id = new EntityId(++mEntityMaxId);

				// always override the "id"
				entity["id"] = id;

				// add to hashtable
				mEntities.Add(id, entity);
			}

			return mEntityMaxId;
		}

		//-------------------------------------------------------------
		/// <summary>
		/// Removes an entity
		/// </summary>
		/// <param name="id">The entity id to remove</param>
		/// <returns>True if the entity could be removed</returns>
		//-------------------------------------------------------------
		public bool Remove(EntityId id)
		{
			lock(mEntities)
			{
				if (!mEntities.Contains(id))
					return false;

				mEntities.Remove(id);
			}

			return true;
		}

		
		//-------------------------------------------------------------
		/// <summary>
		/// Returns the list of entities (types, ids, and descriptions)
		/// </summary>
		/// <returns>A list of EntityDesc</returns>
		//-------------------------------------------------------------
		public ArrayList Enumerate()
		{
			lock(mEntities)
			{
				ArrayList list = new ArrayList(mEntities.Count);

				foreach(DictionaryEntry entry in mEntities)
				{
					EntityDesc desc;
					
					desc.mId = (EntityId) entry.Key;

					IDictionary dict = (IDictionary)entry.Value;
					desc.mType = (string) dict["type"];
					desc.mDesc = (string) dict["desc"];
				}
			}

			return null;
		}


		// ---------- Private Methods -------------


		// ---------- Private Attributes -------------

		// Entities storage
		// hash-key: index of entity,
		// hash-value: an IDictionary object.
		// Thread-safety: ms-help://MS.VSCC.2003/MS.MSDNQTR.2003APR.1033/cpguide/html/cpconcollectionssynchronizationthreadsafety.htm
		protected internal Hashtable mEntities;

		// Next free id for entities
		protected internal EntityId mEntityMaxId = 0;

	} // Entities

}


//---------------------------------------------------------------
//
//	$Log: Entities.cs,v $
//	Revision 1.1  2003/06/10 04:42:30  ralf
//	Conforming to .Net naming rules
//	
//	Revision 1.1.1.1  2003/06/08 18:49:55  ralf
//	Initial revision
//	
//---------------------------------------------------------------

