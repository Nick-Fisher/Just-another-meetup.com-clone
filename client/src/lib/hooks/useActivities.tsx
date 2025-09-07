import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import httpClient from '../api/httpClient';

export const useActivities = () => {
  const queryClient = useQueryClient();

  const { data: activities, isPending } = useQuery({
    queryKey: ['activities'],
    queryFn: async () => {
      const response = await httpClient.get<Activity[]>('/meetings');

      return response.data;
    },
  });

  const updateActivitiy = useMutation({
    mutationFn: async (activity: Activity) => {
      await httpClient.put('/meetings', activity);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['activities'],
      });
    },
  });

  const createActivity = useMutation({
    mutationFn: async (activity: Activity) => {
      await httpClient.post('/meetings', activity);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['activities'],
      });
    },
  });

  const deleteActivity = useMutation({
    mutationFn: async (id: string) => {
      await httpClient.delete(`/meetings/${id}`);
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
    updateActivitiy,
    createActivity,
    deleteActivity,
  };
};
