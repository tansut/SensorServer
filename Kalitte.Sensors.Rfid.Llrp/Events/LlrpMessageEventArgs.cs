using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Llrp.Core;
using Kalitte.Sensors.Rfid;
using Kalitte.Sensors.Exceptions;

namespace Kalitte.Sensors.Rfid.Llrp.Events
{
public class LlrpMessageEventArgs : EventArgs
{
    // Fields
    private SensorProviderException m_exception;
    private LlrpMessageBase m_message;
    private LlrpMessageType m_messageType;
    private uint m_requestId;

    // Methods
    public LlrpMessageEventArgs(LlrpMessageBase messageResponse)
    {
        if (messageResponse == null)
        {
            throw new ArgumentNullException("messageResponse");
        }
        this.m_message = messageResponse;
        this.m_requestId = messageResponse.Id;
        this.m_messageType = messageResponse.MessageType;
    }

    public LlrpMessageEventArgs(uint id, LlrpMessageType messageType, SensorProviderException exception)
    {
        if (exception == null)
        {
            throw new ArgumentNullException("exception");
        }
        this.m_requestId = id;
        this.m_exception = exception;
        this.m_messageType = messageType;
    }

    // Properties
    public SensorProviderException Exception
    {
        get
        {
            return this.m_exception;
        }
    }

    public bool IsSuccess
    {
        get
        {
            return (null == this.m_exception);
        }
    }

    public LlrpMessageType MessageType
    {
        get
        {
            return this.m_messageType;
        }
    }

    public uint RequestId
    {
        get
        {
            return this.m_requestId;
        }
    }

    public LlrpMessageBase Response
    {
        get
        {
            return this.m_message;
        }
    }
}

 

}
