namespace MOE.Common.Models.Repositories
{
    public interface IApproachSpeedAggregationRepository
    {
        void Add(ApproachSpeedAggregationData approachSpeedAggregationData);
        void Remove(ApproachSpeedAggregationData approachSpeedAggregationData);
        void Remove(int id);
        void Update(ApproachSpeedAggregationData approachSpeedAggregationData);
    }
}