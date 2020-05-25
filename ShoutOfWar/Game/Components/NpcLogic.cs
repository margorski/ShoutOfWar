using Microsoft.Xna.Framework;
using MonoGame.Extended;
using ShoutOfWar.Engine;
using ShoutOfWar.Game.Components.General;
using ShoutOfWar.Game.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoutOfWar.Game.Components
{
    public class NpcLogic : Component
    {
        public enum Team
        {
            None,
            Blue,
            Red,
            Green,
            Yellow
        }
        public Team team;
        private Dictionary<Team, Color?> teamColors = new Dictionary<Team, Color?>
        {
            {  Team.None, null },
            {  Team.Blue, Color.Blue },
            {  Team.Green, Color.Green },
            {  Team.Red, Color.Red },
            {  Team.Yellow, Color.Yellow }
        };

        private Dynamic dynamic;
        private double facingAngle;

        public int randomMoveDelay;
        public int randomMoveMaxLength;

        public bool UnderCommand {
            get
            {
                return commander != null;
            }
        }


        // stats
        private float maxHp = 300;
        private float hp = 0;
        private float def = 10;
        private float attack = 20;
        private int attackDelay = 500;
        //
        private double attackTimer = 0.0;

        private Vector2? targetPosition = null;
        private Entity commander;
        private Entity attackTarget;

        private double randomLoiterCurrentTime = 0.0;
        private AnimatedSprite animatedSprite;

        public const double veryNarrowVisibilityCone = Math.PI / 6.0;
        public const double narrowVisibilityCone = Math.PI / 4.0;
        public const double normalVisibilityCone = Math.PI / 2.0;
        public const double wideVisibilityCone = Math.PI * 3.0 / 2.0;

        public double[] visibilityCones;
        public double VisibilityCone {
            get
            {
                return visibilityCones[selectedVisibilityCone];
            }
        }
        public int selectedVisibilityCone = 0;
 
        public float moveSpeed = 150.0f;
        public int visibilityRadius = 250;
        public float separationRadius = 50.0f;


        private Vector2 coherency;
        private Vector2 separation;
        private Vector2 alignment;

        public NpcLogic(int randomMoveDelay, int randomMoveMaxLength, Team team = Team.None)
        {
            this.team = team;
            this.randomMoveDelay = randomMoveDelay;
            this.randomMoveMaxLength = randomMoveMaxLength;
            randomLoiterCurrentTime = Game1.Current.Random.Next(this.randomMoveDelay);
            
            hp = maxHp;

            visibilityCones = new double[] { wideVisibilityCone, normalVisibilityCone, narrowVisibilityCone, veryNarrowVisibilityCone };
            ResetVisibilityCone();
    }

    public override void Draw(GameTime gameTime)
        {
            if (UnderCommand) DrawSelectedMarker(6, 15);
            DrawHpBar(30, 7, hp, maxHp);
        }
        
        public void DrawSelectedMarker(int height, int width)
        {
            Game1.Current.SpriteBatch.DrawEllipse(parent.position + new Vector2(2, 10), new Vector2(width, height), 128, Color.LawnGreen);
        }

        public void DrawHpBar(int hpBarLength, int hpBarHeight, float hp, float maxHp)
        {

            var baseX = (int)parent.position.X + 4 - hpBarLength / 2;
            var baseY = (int)parent.position.Y + 18;

            var hpRatio = hp / maxHp;
            var greenBarLength = (int)((hpBarLength - 2) * hpRatio);
            var redBarLength = (int)((hpBarLength - 2) * (1.0f - hpRatio));

            Game1.Current.SpriteBatch.Draw(Util.pixelTexture, new Rectangle(baseX, baseY, hpBarLength, hpBarHeight), Color.Black);
            Game1.Current.SpriteBatch.Draw(Util.pixelTexture, new Rectangle(baseX + 1, baseY + 1, greenBarLength, hpBarHeight - 2), Color.Green);
            Game1.Current.SpriteBatch.Draw(Util.pixelTexture, new Rectangle(baseX + 1 + greenBarLength, baseY + 1, redBarLength, hpBarHeight - 2), Color.Red);
        }

        public override void Init()
        {
        }
        
        public override void Update(GameTime gameTime)
        {
            if (dynamic == null)
            {
                dynamic = parent.GetComponent<Dynamic>();
                if (dynamic == null) return;
            }

            if (animatedSprite == null)
            {
                animatedSprite = parent.GetComponent<AnimatedSprite>();
                if (animatedSprite == null) return;
            }

            animatedSprite.color = teamColors[team];

            if (hp <= 0)
            {
                this.parent.enabled = false;
                return;
            }

            //RandomizeLoiterTarget(gameTime);
            UpdateRotation();

            dynamic.Acceleration = coherency = alignment = separation = Vector2.Zero;                        
            if (UnderCommand)
            {
                var neighboursOnWideRange = GetNeighbours(narrowVisibilityCone);
                if (neighboursOnWideRange.Count > 0)
                {
                    var neighbours = GetNeighbours(normalVisibilityCone);

                    targetPosition = null;                    

                    // selected but see neighbours, flocking algorithm
                    neighbours.ForEach(e =>
                    {
                        var velocity = e.GetComponent<Dynamic>();
                        if (velocity != null && !double.IsNaN(velocity.Velocity.X) && !double.IsNaN(velocity.Velocity.Y))
                        {
                            alignment += velocity.Velocity;
                            coherency += e.position;
                            var deltaPosition = (parent.position - e.position);
                            if (deltaPosition.Length() < separationRadius)
                            {
                                separation += Vector2.Normalize(deltaPosition) / deltaPosition.Length();
                            }
                        }
                    });

                    var forceLimit = 20.0f;

                    const float coherencyWeight = 5.0f;
                    const float alignWeight = 1.0f;
                    const float separationWeight = 10000.0f;

                    coherency /= neighbours.Count;
                    coherency -= parent.position;

                    // temp
                    var d = coherency.Length();
                    if (d < 100.0) 
                        coherency *= (d / 100.0f);
                    // temp off
                    if (coherency.Length() > forceLimit)
                    {
                        coherency.Normalize();
                        coherency *= forceLimit;
                    }
                    coherency *= coherencyWeight;

                    alignment /= neighbours.Count;
                    if (alignment.Length() > forceLimit)
                    {
                        alignment.Normalize();
                        alignment *= forceLimit;
                    }
                    alignment *= alignWeight;

                    separation /= neighbours.Count;
                    separation *= separationWeight;

                    dynamic.Acceleration = (coherency + alignment + separation);
                }
                else
                {                    
                    // selected but leader, go directly
                    targetPosition = commander.position;
                }
            }
            else
            {
                // stop movement if enemy was killed
                if (attackTarget != null)
                {
                    var targetNpcLogic = attackTarget.GetComponent<NpcLogic>();
                    if (targetNpcLogic.hp == 0) EndTargetMovement();
                }
                
                // search for closest enemy
                attackTarget = null;
                var enemiesInRange = Game1.Current.EnabledEntities.FindAll(e =>
                {
                    var distanceToEnemy = (parent.position - e.position).Length();
                    var npcLogic = e.GetComponent<NpcLogic>();

                    return npcLogic != null && npcLogic.team != team && distanceToEnemy <= visibilityRadius && npcLogic.hp > 0;
                });
                enemiesInRange.Sort((e1, e2) =>
                {
                    var distance1 = (parent.position - e1.position).Length();
                    var distance2 = (parent.position - e2.position).Length();
                    return distance1.CompareTo(distance2);
                });
                if (enemiesInRange.Count > 0) attackTarget = enemiesInRange[0];
                

                // have an enemy
                if (attackTarget != null)
                {
                    var attackDistance = 30.0;
                    var distanceToEnemy = (parent.position - attackTarget.position).Length();
                    if (distanceToEnemy <= attackDistance)
                    {
                        //attack
                        EndTargetMovement();
                        attackTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (attackTimer >= attackDelay) {
                            attackTimer = 0.0;
                            Attack();
                        }
                    }
                    else
                    {
                        targetPosition = attackTarget.position;
                    }

                }
            }

            if (targetPosition != null)
            {               
                // random move to target
                MoveToTarget(gameTime);
            }
        }

        private void Attack()
        {
            var backstabAttackAngleRange = Math.PI / 6;
            var targetNpcLogic = attackTarget.GetComponent<NpcLogic>();
            if (targetNpcLogic == null || targetNpcLogic.hp <= 0) return;

            //targetNpcLogic.facingAngle;
            var backstab = Util.IsInRadiusRange(facingAngle, backstabAttackAngleRange, targetNpcLogic.facingAngle);
            if (backstab)
            {
                var test = false;
            }
            var attackMultiplier = 1 + 0.3f * (backstab ? 1 : 0);
            targetNpcLogic.hp -= MathHelper.Clamp(attack * attackMultiplier - targetNpcLogic.def, 0, targetNpcLogic.hp);
        }

        private void UpdateRotation()
        {
            if (dynamic != null && !float.IsNaN(dynamic.Velocity.X) && !float.IsNaN(dynamic.Velocity.Y) && dynamic.Velocity != Vector2.Zero)
            {

                facingAngle = Util.GetVectorAngleRad(dynamic.Velocity);
            }
        }

        private List<Entity> GetNeighbours(double visibilityAngle = 0.0)
        {
            var neighbours = new List<Entity>();

            if (commander != null) {
                var army = commander.GetComponent<PlayerLogic>().army;
                neighbours = army.FindAll(e =>
                {
                    // filter same entity
                    if (e == parent) return false;

                    // filter not moving entity
                    var dynamic = e.GetComponent<Dynamic>();
                    if (dynamic == null || dynamic.Velocity.Length() == 0.0f) return false;

                    var relPoint = e.position - parent.position;                    
                    var neighbourAngle = Util.GetVectorAngleRad(relPoint);

                    return Util.IsWithinRadius(relPoint, visibilityRadius) && (visibilityAngle == 0.0 || Util.IsInRadiusRange(facingAngle, visibilityAngle, neighbourAngle));
                });
            }
            return neighbours;
        }
        
        public void FollowCommander(Entity commander, int visibilityCone)
        {            
            this.commander = commander;
            attackTarget = null;
            attackTimer = 0.0;
            selectedVisibilityCone = visibilityCone;
        }

        private void MoveToTarget(GameTime gameTime)
        {            
            var remainingDistance = targetPosition.Value - parent.position;
            dynamic.Velocity = moveSpeed * Vector2.Normalize(remainingDistance);

            var deltaDistance = dynamic.Velocity * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);

            if (deltaDistance.Length() >= remainingDistance.Length())
            {               
                if (targetPosition.HasValue) parent.position = targetPosition.Value;
                EndTargetMovement();
            }
        }

        private void RandomizeLoiterTarget(GameTime gameTime)
        {
            if (targetPosition != null || UnderCommand) return;

            randomLoiterCurrentTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (randomLoiterCurrentTime >= randomMoveDelay)
            {
                randomLoiterCurrentTime = Game1.Current.Random.Next(this.randomMoveDelay);
                targetPosition = new Vector2(
                    Game1.Current.Random.Next((int)parent.position.X - randomMoveMaxLength, (int)parent.position.X + randomMoveMaxLength),
                    Game1.Current.Random.Next((int)parent.position.Y - randomMoveMaxLength, (int)parent.position.Y + randomMoveMaxLength)
                );
            }
        }

        private void EndTargetMovement()
        {
            targetPosition = null;
            dynamic.Velocity = Vector2.Zero;
            dynamic.Acceleration = Vector2.Zero;
            randomLoiterCurrentTime = Game1.Current.Random.Next(this.randomMoveDelay);
        }

        public void ResetVisibilityCone()
        {
            selectedVisibilityCone = 0;
        }

        public void ReleaseFromCommand()
        {
            commander = null;            
            EndTargetMovement();
            ResetVisibilityCone();
        }

        public override void DrawDebug(GameTime gameTime)
        {
            Game1.Current.SpriteBatch.DrawCircle(parent.position, 1.0f, 128, Color.Red, 5);

            Game1.Current.SpriteBatch.DrawCircle(parent.position, separationRadius, 128, Color.Red, 1);
            Game1.Current.SpriteBatch.DrawLine(parent.position, dynamic.Acceleration.Length(), (float)facingAngle, Color.Pink, 3);

            Game1.Current.SpriteBatch.DrawLine(parent.position, parent.position + coherency, Color.Yellow, 2);
            Game1.Current.SpriteBatch.DrawLine(parent.position, parent.position + separation, Color.Red, 2);
            Game1.Current.SpriteBatch.DrawLine(parent.position, parent.position + alignment, Color.Green, 2);


            Game1.Current.SpriteBatch.DrawLine(parent.position, visibilityRadius, (float)(facingAngle - VisibilityCone / 2.0), Color.Blue, 1);
            Game1.Current.SpriteBatch.DrawLine(parent.position, visibilityRadius, (float)(facingAngle + VisibilityCone / 2.0), Color.Blue, 1);


            Game1.Current.SpriteBatch.DrawLine(parent.position, visibilityRadius, (float)(facingAngle - normalVisibilityCone / 2.0), Color.Gray, 1);
            Game1.Current.SpriteBatch.DrawLine(parent.position, visibilityRadius, (float)(facingAngle + normalVisibilityCone / 2.0), Color.Gray, 1);            


            //Game1.Current.SpriteBatch.DrawLine(parent.position, dynamic.Velocity.Length()/5.0f, (float)facingAngle, Color.GreenYellow);            
        }
    }
}
