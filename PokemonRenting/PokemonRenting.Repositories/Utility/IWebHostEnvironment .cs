using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace PokemonRenting.Repositories.Utility
{
    public interface IWebHostEnvironment : IHostEnvironment
    {
        /// <summary>
        /// Gets or sets the absolute path to the directory that contains the web-servable application content files.
        /// This defaults to the 'wwwroot' subfolder.
        /// </summary>
        string WebRootPath { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="IFileProvider"/> pointing at <see cref="WebRootPath"/>.
        /// This defaults to referencing files from the 'wwwroot' subfolder.
        /// </summary>
        IFileProvider WebRootFileProvider { get; set; }
    }
}
