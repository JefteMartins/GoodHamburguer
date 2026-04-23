using System.Reflection;
using FluentAssertions;
using GoodHamburguer.Infrastructure.Persistence.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace GoodHamburguer.UnitTests;

public class RefactorOrderSlotsToItemCodesMigrationTests
{
    [Fact]
    public void Up_ShouldCreateExpectedSchemaAndDataOperations()
    {
        var migration = new RefactorOrderSlotsToItemCodes();
        var builder = new MigrationBuilder("MySql");

        InvokeMigrationMethod(migration, "Up", builder);

        builder.Operations.OfType<AddColumnOperation>().Count().Should().Be(3);
        builder.Operations.OfType<CreateIndexOperation>().Count().Should().Be(3);
        builder.Operations.OfType<AddForeignKeyOperation>().Count().Should().Be(3);
        builder.Operations.OfType<DropColumnOperation>().Count().Should().Be(6);

        var sql = builder.Operations.OfType<SqlOperation>().Single().Sql;
        sql.Should().Contain("UPDATE orders");
        sql.Should().Contain("SandwichItemCode");
        sql.Should().Contain("DrinkItemCode");
    }

    [Fact]
    public void Down_ShouldRestoreLegacyColumnsAndDropRefactorArtifacts()
    {
        var migration = new RefactorOrderSlotsToItemCodes();
        var builder = new MigrationBuilder("MySql");

        InvokeMigrationMethod(migration, "Down", builder);

        builder.Operations.OfType<AddColumnOperation>().Count().Should().Be(6);
        builder.Operations.OfType<DropForeignKeyOperation>().Count().Should().Be(3);
        builder.Operations.OfType<DropIndexOperation>().Count().Should().Be(3);
        builder.Operations.OfType<DropColumnOperation>().Count().Should().Be(3);

        var sql = builder.Operations.OfType<SqlOperation>().Single().Sql;
        sql.Should().Contain("UPDATE orders");
        sql.Should().Contain("SandwichName");
        sql.Should().Contain("DrinkUnitPrice");
    }

    private static void InvokeMigrationMethod(Migration migration, string methodName, MigrationBuilder builder)
    {
        var method = migration.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
        method.Should().NotBeNull();
        method!.Invoke(migration, [builder]);
    }
}
