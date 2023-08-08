using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Rfid.Core
{

    public static class RfidErrorCode
    {


        private const int TagNotFoundValue = 100;
        private const int TagCorruptedValue = 101;
        private const int TagDataLostValue = 102;
        private const int ReadOnlyTagValue = 103;
        private const int LockedTagValue = 104;
        private const int LockFailedValue = 105;
        private const int PartiallyLockedValue = 106;
        private const int InvalidReadFilterValue = 107;
        private const int FilterNotFoundValue = 108;
        private const int TagHasNoDataValue = 109;
        private const int TagHasNoUserMemoryValue = 110;
        private const int InvalidPrintLabelValue = 111;
        private const int UnlockFailedValue = 112;
        private const int PaperOutValue = 113;
        private const int PaperJamValue = 114;
        private const int RibbonFaultValue = 116;
        private const int RibbonBrokenValue = 117;
        private const int GapNotDetectedValue = 118;
        private const int CutterFaultValue = 119;

        public static readonly ErrorCode TagNotFound;
        public static readonly ErrorCode TagCorrupted;
        public static readonly ErrorCode TagDataLost;
        public static readonly ErrorCode ReadOnlyTag;
        public static readonly ErrorCode LockedTag;
        public static readonly ErrorCode LockFailed;
        public static readonly ErrorCode UnlockFailed;
        public static readonly ErrorCode PartiallyLocked;
        public static readonly ErrorCode InvalidReadFilter;
        public static readonly ErrorCode FilterNotFound;
        public static readonly ErrorCode TagHasNoData;
        public static readonly ErrorCode TagHasNoUserMemory;
        public static readonly ErrorCode PaperOut;
        public static readonly ErrorCode PaperJam;
        public static readonly ErrorCode RibbonFault;
        public static readonly ErrorCode RibbonBroken;
        public static readonly ErrorCode GapNotDetected;
        public static readonly ErrorCode CutterFault;
        public static readonly ErrorCode InvalidPrintLabel;



        static RfidErrorCode()
        {
            TagNotFound = new ErrorCode(TagNotFoundValue, "TagNotFound");
            TagCorrupted = new ErrorCode(TagCorruptedValue, "TagCorrupted");
            TagDataLost = new ErrorCode(TagDataLostValue, "TagDataLost");
            ReadOnlyTag = new ErrorCode(ReadOnlyTagValue, "ReadOnlyTag");
            LockedTag = new ErrorCode(LockedTagValue, "LockedTag");
            LockFailed = new ErrorCode(LockFailedValue, "LockFailed");
            UnlockFailed = new ErrorCode(UnlockFailedValue, "UnlockFailed");
            PartiallyLocked = new ErrorCode(PartiallyLockedValue, "PartiallyLocked");
            InvalidReadFilter = new ErrorCode(InvalidReadFilterValue, "InvalidReadFilter");
            FilterNotFound = new ErrorCode(FilterNotFoundValue, "FilterNotFound");
            TagHasNoData = new ErrorCode(TagHasNoDataValue, "TagHasNoData");
            InvalidPrintLabel = new ErrorCode(InvalidPrintLabelValue, "InvalidPrintLabel");

            TagHasNoUserMemory = new ErrorCode(TagHasNoUserMemoryValue, "TagHasNoUserMemory");
            PaperOut = new ErrorCode(PaperOutValue, "PaperOut");
            PaperJam = new ErrorCode(PaperJamValue, "PaperJam");
            RibbonFault = new ErrorCode(RibbonFaultValue, "RibbonFault");
            RibbonBroken = new ErrorCode(RibbonBrokenValue, "RibbonBroken");
            GapNotDetected = new ErrorCode(GapNotDetectedValue, "GapNotDetected");
            CutterFault = new ErrorCode(CutterFaultValue, "CutterFault");

        }


    }
}
