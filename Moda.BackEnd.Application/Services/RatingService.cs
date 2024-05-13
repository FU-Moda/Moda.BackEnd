using AutoMapper;
using Moda.BackEnd.Application.IRepositories;
using Moda.Backend.Domain.Models;
using Moda.BackEnd.Application.IServices;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moda.BackEnd.Common.Utils;

namespace Moda.BackEnd.Application.Services
{
    public class RatingService : GenericBackendService, IRatingService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Rating> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public RatingService(
            IServiceProvider serviceProvider, IMapper mapper, IRepository<Rating> repository, IUnitOfWork unitOfWork
            ) : base(serviceProvider)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<AppActionResult> CreateRating(CreateRatingRequest dto)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var firebaseService = Resolve<IFirebaseService>();
                var staticFileRepository = Resolve<IRepository<StaticFile>>();
                var ratingDb = _mapper.Map<Rating>( dto );
                ratingDb.Id = Guid.NewGuid();
                ratingDb.CreateDate = DateTime.Now;
                await _repository.Insert(ratingDb);
                await _unitOfWork.SaveChangesAsync();

                if(dto.Img != null)
                {
                    Random random = new Random();
                    string pathName = null;
                    foreach (var img in dto.Img!)
                    {
                        pathName = SD.FirebasePathName.RATING_PREFIX + $"{ratingDb.Id} {random.Next(10000)}.jpg";
                        var upload = await firebaseService!.UploadFileToFirebase(img, pathName);
                        if (upload.Result != null)
                        {
                            await staticFileRepository!.Insert(new StaticFile
                            {
                                RatingId = ratingDb.Id,
                                Img = upload!.Result!.ToString()!,
                            });
                        } else
                        {
                            result.Messages.Add($"Xảy ra lỗi trong khi tải ảnh, một vài ảnh không thể lưu");
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                }


            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> DeleteRating(Guid Id)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var rating = await _repository.GetByExpression(r => r.Id == Id);
                if (rating != null)
                {
                    var staticFileRepository = Resolve<IRepository<StaticFile>>();
                    var staticFile = await staticFileRepository!.GetByExpression(s => s.RatingId == rating.Id);
                    if (staticFile != null)
                    {
                        await staticFileRepository.DeleteById(staticFile.Id);
                    }
                    await _repository.DeleteById(Id);
                } else
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy đánh giá với id {Id}");
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> UpdateRating(UpdateRatingDto dto)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var ratingDb = await _repository.GetById(dto.Id);
                if(ratingDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy đánh giá với id {dto.Id}");
                    return result;
                }
                var firebaseService = Resolve<IFirebaseService>();
                var staticFileRepository = Resolve<IRepository<StaticFile>>();
                ratingDb = _mapper.Map<Rating>(dto);
                ratingDb.CreateDate = DateTime.Now;
                await _repository.Update(ratingDb);
                await _unitOfWork.SaveChangesAsync();

                if (dto.Img != null)
                {
                    Random random = new Random();
                    string pathName = null;
                    var staticDb = await staticFileRepository!.GetAllDataByExpression(s => s.RatingId == dto.Id, 0, 0, null, false, null);
                    if (staticDb.Items != null && staticDb.Items.Count > 0)
                    {
                        foreach (var item in staticDb.Items)
                        {
                            await firebaseService!.DeleteFileFromFirebase(item.Img);
                        }
                    }
                    foreach (var img in dto.Img!)
                    {
                        pathName = SD.FirebasePathName.RATING_PREFIX + $"{ratingDb.Id} {random.Next(10000)}.jpg";
                        var upload = await firebaseService!.UploadFileToFirebase(img, pathName);
                        if (upload.Result != null)
                        {
                            await staticFileRepository!.Insert(new StaticFile
                            {
                                RatingId = ratingDb.Id,
                                Img = upload!.Result!.ToString()!,
                            });
                        }
                        else
                        {
                            result.Messages.Add($"Xảy ra lỗi trong khi tải ảnh, một vài ảnh không thể lưu");
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                }


            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
