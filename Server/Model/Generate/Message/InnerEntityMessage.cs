using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
/// <summary>
/// 传送unit
/// </summary>
	[ResponseType(typeof(M2M_TrasferUnitResponse))]
	[Message(InnerEntityOpcode.M2M_TrasferUnitRequest)]
	[ProtoContract]
	public partial class M2M_TrasferUnitRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public Unit Unit { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

	}

	[Message(InnerEntityOpcode.M2M_TrasferUnitResponse)]
	[ProtoContract]
	public partial class M2M_TrasferUnitResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long InstanceId { get; set; }

	}

}
