Imports System.Management
Imports System
Public Class GetMBServerMACAddress

    Private Shared Function ServerName() As String
        Dim HostName As String
        Dim IpCheck = System.Net.Dns.GetHostEntry("kw000--3")
        HostName = IpCheck.HostName
        Return HostName
    End Function

    Public Shared Function GetMACAddress() As String
        Try
            Dim MACAddress As String
            Dim ComputerName = ServerName()
            Dim theManagementScope As New ManagementScope("\\" & ComputerName & "\root\cimv2")
            Const theQueryString As String = "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 1"
            theManagementScope.Options.Timeout = TimeSpan.FromSeconds(10)

            Dim theObjectQuery As New ObjectQuery(theQueryString)

            Dim theSearcher As New ManagementObjectSearcher(theManagementScope, theObjectQuery)
            Dim theResultsCollection As ManagementObjectCollection = theSearcher.Get()

            For Each currentResult As ManagementObject In theResultsCollection
                MessageBox.Show(currentResult("MacAddress").ToString())
                MACAddress = (currentResult("MacAddress").ToString())
            Next
            Return MACAddress
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Function

End Class

