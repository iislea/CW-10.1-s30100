using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public partial class YourDbContext : DbContext
{
    public YourDbContext()
    {
    }

    public YourDbContext(DbContextOptions<YourDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BorrowedBook> BorrowedBooks { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientTrip> ClientTrips { get; set; }

    public virtual DbSet<Contribution> Contributions { get; set; }

    public virtual DbSet<Copy> Copies { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Edition> Editions { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<LibraryCard> LibraryCards { get; set; }

    public virtual DbSet<LibraryMember> LibraryMembers { get; set; }

    public virtual DbSet<LibraryStaff> LibraryStaffs { get; set; }

    public virtual DbSet<PeriodOfTime> PeriodOfTimes { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("s30100");

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Author__70DAFC142CE352BB");

            entity.ToTable("Author");

            entity.Property(e => e.AuthorId)
                .ValueGeneratedNever()
                .HasColumnName("AuthorID");
            entity.Property(e => e.Country).HasMaxLength(255);
            entity.Property(e => e.DateOfBirth).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => new { e.BookId, e.GenreId }).HasName("PK__Book__CDD89272895FBE77");

            entity.ToTable("Book");

            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Book__GenreID__1618BFDF");
        });

        modelBuilder.Entity<BorrowedBook>(entity =>
        {
            entity.HasKey(e => e.BorrowId).HasName("PK__Borrowed__4295F85F83C6F5F9");

            entity.ToTable(tb => tb.HasTrigger("trg_CheckCardExpiration"));

            entity.Property(e => e.BorrowId)
                .ValueGeneratedNever()
                .HasColumnName("BorrowID");
            entity.Property(e => e.CardId).HasColumnName("CardID");
            entity.Property(e => e.CopyId).HasColumnName("CopyID");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");

            entity.HasOne(d => d.Card).WithMany(p => p.BorrowedBooks)
                .HasForeignKey(d => d.CardId)
                .HasConstraintName("FK__BorrowedB__CardI__292B9453");

            entity.HasOne(d => d.Copy).WithMany(p => p.BorrowedBooks)
                .HasForeignKey(d => d.CopyId)
                .HasConstraintName("FK__BorrowedB__CopyI__2837701A");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("Client_pk");

            entity.ToTable("Client");

            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.FirstName).HasMaxLength(120);
            entity.Property(e => e.LastName).HasMaxLength(120);
            entity.Property(e => e.Pesel).HasMaxLength(120);
            entity.Property(e => e.Telephone).HasMaxLength(120);
        });

        modelBuilder.Entity<ClientTrip>(entity =>
        {
            entity.HasKey(e => new { e.IdClient, e.IdTrip }).HasName("Client_Trip_pk");

            entity.ToTable("Client_Trip");

            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_5_Client");

            entity.HasOne(d => d.IdTripNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdTrip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_5_Trip");
        });

        modelBuilder.Entity<Contribution>(entity =>
        {
            entity.HasKey(e => new { e.AuthorId, e.BookBookId, e.BookGenreId }).HasName("PK__Contribu__91FEDA5B00CAA115");

            entity.ToTable("Contribution");

            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.BookBookId).HasColumnName("Book_BookID");
            entity.Property(e => e.BookGenreId).HasColumnName("Book_GenreID");
            entity.Property(e => e.BookTitle)
                .HasMaxLength(255)
                .HasColumnName("Book_Title");

            entity.HasOne(d => d.Author).WithMany(p => p.Contributions)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contribut__Autho__1ADD74FC");

            entity.HasOne(d => d.Book).WithMany(p => p.Contributions)
                .HasForeignKey(d => new { d.BookBookId, d.BookGenreId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contribution__1BD19935");
        });

        modelBuilder.Entity<Copy>(entity =>
        {
            entity.HasKey(e => e.CopyId).HasName("PK__Copy__C26CCCE5AEF7153A");

            entity.ToTable("Copy");

            entity.Property(e => e.CopyId)
                .ValueGeneratedNever()
                .HasColumnName("CopyID");
            entity.Property(e => e.EditionId).HasColumnName("EditionID");

            entity.HasOne(d => d.Edition).WithMany(p => p.Copies)
                .HasForeignKey(d => d.EditionId)
                .HasConstraintName("FK__Copy__EditionID__255B036F");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.IdCountry).HasName("Country_pk");

            entity.ToTable("Country");

            entity.Property(e => e.Name).HasMaxLength(120);

            entity.HasMany(d => d.IdTrips).WithMany(p => p.IdCountries)
                .UsingEntity<Dictionary<string, object>>(
                    "CountryTrip",
                    r => r.HasOne<Trip>().WithMany()
                        .HasForeignKey("IdTrip")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Trip"),
                    l => l.HasOne<Country>().WithMany()
                        .HasForeignKey("IdCountry")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Country"),
                    j =>
                    {
                        j.HasKey("IdCountry", "IdTrip").HasName("Country_Trip_pk");
                        j.ToTable("Country_Trip");
                    });
        });

        modelBuilder.Entity<Edition>(entity =>
        {
            entity.HasKey(e => e.EditionId).HasName("PK__Edition__C762234332E1FADA");

            entity.ToTable("Edition");

            entity.Property(e => e.EditionId)
                .ValueGeneratedNever()
                .HasColumnName("EditionID");
            entity.Property(e => e.BookAuthorId).HasColumnName("Book_AuthorID");
            entity.Property(e => e.BookBookId).HasColumnName("Book_BookID");
            entity.Property(e => e.BookGenreId).HasColumnName("Book_GenreID");
            entity.Property(e => e.BookTitle)
                .HasMaxLength(255)
                .HasColumnName("Book_Title");
            entity.Property(e => e.Language).HasMaxLength(255);
            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

            entity.HasOne(d => d.BookAuthor).WithMany(p => p.Editions)
                .HasForeignKey(d => d.BookAuthorId)
                .HasConstraintName("FK__Edition__Book_Au__227E96C4");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Editions)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__Edition__Publish__20964E52");

            entity.HasOne(d => d.Book).WithMany(p => p.Editions)
                .HasForeignKey(d => new { d.BookBookId, d.BookGenreId })
                .HasConstraintName("FK__Edition__218A728B");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genre__0385055EA62642BB");

            entity.ToTable("Genre", tb => tb.HasTrigger("trg_PreventGenreDelete"));

            entity.Property(e => e.GenreId)
                .ValueGeneratedNever()
                .HasColumnName("GenreID");
            entity.Property(e => e.GenreName).HasMaxLength(255);
        });

        modelBuilder.Entity<LibraryCard>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PK__LibraryC__55FECD8E1E78DE2B");

            entity.ToTable("LibraryCard", tb => tb.HasTrigger("trg_SetCreationDate"));

            entity.Property(e => e.CardId)
                .ValueGeneratedNever()
                .HasColumnName("CardID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");

            entity.HasOne(d => d.Member).WithMany(p => p.LibraryCards)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK__LibraryCa__Membe__11540AC2");
        });

        modelBuilder.Entity<LibraryMember>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__LibraryM__0CF04B38CC9771D8");

            entity.ToTable("LibraryMember");

            entity.Property(e => e.MemberId)
                .ValueGeneratedNever()
                .HasColumnName("MemberID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
        });

        modelBuilder.Entity<LibraryStaff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__LibraryS__96D4AAF7CC4063CA");

            entity.ToTable("LibraryStaff");

            entity.Property(e => e.StaffId)
                .ValueGeneratedNever()
                .HasColumnName("StaffID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__LibrarySt__Manag__2C0800FE");
        });

        modelBuilder.Entity<PeriodOfTime>(entity =>
        {
            entity.HasKey(e => e.NameOfPeriod).HasName("PK__Period_o__DA47D7491E4755C2");

            entity.ToTable("Period_of_time");

            entity.Property(e => e.NameOfPeriod).HasMaxLength(255);
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("PK__Publishe__4C657E4BA2DBFC1C");

            entity.ToTable("Publisher");

            entity.Property(e => e.PublisherId)
                .ValueGeneratedNever()
                .HasColumnName("PublisherID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.PublisherName).HasMaxLength(255);
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.IdTrip).HasName("Trip_pk");

            entity.ToTable("Trip");

            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DateTo).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(220);
            entity.Property(e => e.Name).HasMaxLength(120);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
