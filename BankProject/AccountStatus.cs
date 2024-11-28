using System;

[Flags]
public enum AccountStatus : byte
{
	Open = 0,
	Closed = 1,
	Bankrupt = 2,
}