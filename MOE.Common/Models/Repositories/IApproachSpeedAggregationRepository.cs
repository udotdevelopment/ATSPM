namespace MOE.Common.Models.Repositories
{
    public interface IApproachSpeedAggregationRepository
    {
        void Add(ApproachSpeedAggregation approachSpeedAggregation);
        void Remove(ApproachSpeedAggregation approachSpeedAggregation);
        void Remove(int id);
        void Update(ApproachSpeedAggregation approachSpeedAggregation);
    }
}