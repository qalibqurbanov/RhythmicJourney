using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmicJourney.Core.Entities.Music;

namespace RhythmicJourney.Application.Features.Category.Common.DTOs;

public record AddCategoryRequestDTO(string CategoryName);