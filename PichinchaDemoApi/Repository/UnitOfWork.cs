using System;
using PichinchaDemoApi.Models;
using PichinchaDemoApi.Data;

namespace PichinchaDemoApi.Repository;

public class UnitOfWork : IDisposable
{
    private DataContext _context;
    private GenericRepository<Cuenta>? cuentaRepository;
    private GenericRepository<Cliente>? clienteRepository;
    private GenericRepository<Movimiento>? movimientoRepository;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }

    public GenericRepository<Cuenta> CuentaRepository
    {
        get
        {

            if (this.cuentaRepository == null)
            {
                this.cuentaRepository = new GenericRepository<Cuenta>(_context);
            }
            return cuentaRepository;
        }
    }

    public GenericRepository<Cliente> ClienteRepository
    {
        get
        {

            if (this.clienteRepository == null)
            {
                this.clienteRepository = new GenericRepository<Cliente>(_context);
            }
            return clienteRepository;
        }
    }

    public GenericRepository<Movimiento> MovimientoRepository
    {
        get
        {

            if (this.movimientoRepository == null)
            {
                this.movimientoRepository = new GenericRepository<Movimiento>(_context);
            }
            return movimientoRepository;
        }
    }

    public async Task Guardar()
    {
        await _context.SaveChangesAsync();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}