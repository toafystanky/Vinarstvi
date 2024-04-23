using System;

/// <summary>
/// Represents an exception that is thrown when a duplicate entry is encountered.
/// </summary>
public class DuplicateEntryException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEntryException"/> class with a default message.
    /// </summary>
    public DuplicateEntryException() : base("Duplicate entry. The record already exists.")
    {
        // Debug statement for constructor entry
        Console.WriteLine("DuplicateEntryException constructor called.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEntryException"/> class with the specified message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public DuplicateEntryException(string message) : base(message)
    {
        // Debug statement for constructor entry with custom message
        Console.WriteLine($"DuplicateEntryException constructor called with message: {message}");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEntryException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public DuplicateEntryException(string message, Exception innerException) : base(message, innerException)
    {
        // Debug statement for constructor entry with custom message and inner exception
        Console.WriteLine($"DuplicateEntryException constructor called with message: {message}, and inner exception: {innerException}");
    }
}