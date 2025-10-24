import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import agent from '../api/agent';
import { useLocation } from 'react-router';
import { useAccount } from './useAccount';

export const useActivities = (id?: string) => {
  const queryClient = useQueryClient();
  const { currentUser } = useAccount();
  const location = useLocation();

  const { data: activities, isLoading } = useQuery({
    queryKey: ['activities'],
    queryFn: async () => {
      const response = await agent.get<Activity[]>('/meetings');
      return response.data;
    },
    enabled: !id && location.pathname === '/activities' && !!currentUser,
    select: (data) => {
      return data.map((activity) => ({
        ...activity,
        isHost: activity.hostId === currentUser?.id,
        isGoing: activity.attendees.some((a) => a.id === currentUser?.id),
      }));
    },
  });

  const { data: activity, isLoading: isLoadingActivity } = useQuery({
    queryKey: ['activities', id],
    queryFn: async () => {
      const response = await agent.get<Activity>(`/meetings/${id}`);
      return response.data;
    },
    enabled: !!id && !!currentUser,
    select: (activity) => ({
      ...activity,
      isHost: activity.hostId === currentUser?.id,
      isGoing: activity.attendees.some((a) => a.id === currentUser?.id),
    }),
  });

  const updateActivity = useMutation({
    mutationFn: async (activity: Activity) => {
      await agent.put('/meetings', activity);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['activities'],
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

  const updateAttendance = useMutation({
    mutationFn: async (id: string) => {
      await agent.post(`/meetings/${id}/attend`);
    },
    onMutate: async (id: string) => {
      await queryClient.cancelQueries({ queryKey: ['activities', id] });

      const oldActivity = queryClient.getQueryData<Activity>([
        'activities',
        id,
      ]);

      queryClient.setQueryData<Activity>(
        ['activities', id],
        (previousActivity) => {
          if (!previousActivity || !currentUser) return previousActivity;

          const isHost = previousActivity.hostId === currentUser.id;
          const isAttending = previousActivity.attendees.some(
            (a) => a.id === currentUser.id
          );

          return {
            ...previousActivity,
            isCancelled: isHost
              ? !previousActivity.isCancelled
              : previousActivity.isCancelled,
            attendees: isAttending
              ? isHost
                ? previousActivity.attendees
                : previousActivity.attendees.filter(
                    (x) => x.id !== currentUser.id
                  )
              : [
                  ...previousActivity.attendees,
                  {
                    id: currentUser.id,
                    displayName: currentUser.displayName,
                    imageUrl: currentUser.imageUrl,
                  },
                ],
          };
        }
      );

      return { oldActivity };
    },
    onError: (_error, _id, context) => {
      if (context?.oldActivity) {
        queryClient.setQueryData<Activity>(
          ['activities', _id],
          context.oldActivity
        );
      }
    },
  });

  return {
    activities,
    isLoading,
    updateActivity,
    createActivity,
    deleteActivity,
    activity,
    isLoadingActivity,
    updateAttendance,
  };
};
