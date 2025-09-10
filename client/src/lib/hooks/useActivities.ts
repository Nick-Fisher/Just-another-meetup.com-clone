import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import agent from '../api/agent';
import { useLocation } from 'react-router';

export const useActivities = (id?: string) => {
  const queryClient = useQueryClient();
  const location = useLocation();

  const { data: activities, isPending } = useQuery({
    queryKey: ['activities'],
    queryFn: async () => {
      const response = await agent.get<Activity[]>('/meetings');
      return response.data;
    },
    staleTime: 1000 * 60 * 1, // 1 minute
    enabled: !id && location.pathname === '/activities',
  });

  const { data: activity, isLoading: isLoadingActivity } = useQuery({
    queryKey: ['activities', id],
    queryFn: async () => {
      const response = await agent.get<Activity>(`/meetings/${id}`);
      return response.data;
    },
    enabled: !!id,
  });

  const updateActivity = useMutation({
    mutationFn: async (activity: Activity) => {
      await agent.put('/meetings', activity);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['meetings'],
      });
    },
  });

  const createActivity = useMutation({
    mutationFn: async (activity: Activity) => {
      const response = await agent.post('/meetings', activity);
      return response.data;
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['activities'],
      });
    },
  });

  const deleteActivity = useMutation({
    mutationFn: async (id: string) => {
      await agent.delete(`/meetings/${id}`);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['activities'],
      });
    },
  });

  return {
    activities,
    isPending,
    updateActivity,
    createActivity,
    deleteActivity,
    activity,
    isLoadingActivity,
  };
};
