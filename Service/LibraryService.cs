﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ozhar_hasfarim.Data;
using ozhar_hasfarim.Enums;
using ozhar_hasfarim.Models;
using ozhar_hasfarim.ViewModels;

namespace ozhar_hasfarim.Service
{
    public class LibraryService : ILibraryService
    {
        private readonly ApplicationDbContext _context;
        public LibraryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LibraryVM>> GetAllLibrary() => 
            await _context.Libraries
            .Select(library => new LibraryVM() { Id = library.Id, Genre = library.Genre })
            .ToListAsync();

        public async Task<LibraryModel?> GetLibraryByID(long id)=>
            await _context.Libraries
            .FirstOrDefaultAsync(library => library.Id == id);

        public async Task<LibraryModel> CreateLibrary(LibraryVM newLibrary)
        {
            if (await GenreExists(newLibrary.Genre))
            {
                throw new Exception("The genre exists. Please try to create a different genre.");
            }
            LibraryModel model = new() { Genre = newLibrary.Genre };
            _context.Libraries.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public void DeleteLibrary(LibraryModel library)
        {
            _context.Libraries.Remove(library);
            _context.SaveChanges();
        }

        public async Task<bool> GenreExists(GenreEnum genre) =>
            await _context.Libraries.AnyAsync(l => l.Genre == genre);
    }
}
