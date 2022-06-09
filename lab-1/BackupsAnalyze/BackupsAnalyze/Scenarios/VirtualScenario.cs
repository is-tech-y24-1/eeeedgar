using Backups.Job;
using Backups.Repo;
using Backups.Tests;
using Backups.Zippers;

namespace BackupsAnalyze.Scenarios
{
    public class VirtualScenario : IScenario
    {
        private BackupJob _job;
        private IRepository _repository;
        private IStorageCreator _zipper;

        private string _workingDirectory;
        private string _localRepositoryPath;
        private string _sourcePath;
        private string _filePath;

        private int _operationNumber;
        
        public void Run()
        {
            Setup();
            
            for (var i = 0; i < _operationNumber; i++)
            {
                var jobObject = new JobObject(_filePath);
                _job.AddJobObject(jobObject);
                
                _job.CreateBackup();
                
                _job.RemoveJobObject(jobObject);
            }
        }
        
        private void Setup()
        {
            _workingDirectory = @"D:\tech\backups\";
            
            _localRepositoryPath = $@"{_workingDirectory}result";
            _sourcePath = $@"{_workingDirectory}sources\";

            _filePath = $@"{_sourcePath}1.txt";
                
            _repository = new LocalRepository(_localRepositoryPath);
            _zipper = new TestStorageCreator();
            
            _job = new BackupJob(_repository, _zipper);

            _operationNumber = 100;
        }
    }
}